using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal class Parser
{
    private List<Token> Tokens { get; init; }
    private int _current = 0;
    
    public Parser(IEnumerable<Token> tokens)
    {
        Tokens = tokens.ToList();
    }

    private Token Peek(int positions = 0) => Tokens.ElementAtOrDefault(_current + positions) ?? Tokens.Last();
    private void Advance() => _current++;

    private bool Match(TokenType[] types) 
    {
        var matched = types.Contains(Peek().Type);
        if (matched) Advance();

        return matched;
    }

    private bool Match(TokenType type)
    {
        var matched = Peek().Type == type;
        if (matched) Advance();

        return matched;
    }

    private bool Match(TokenType type, out Token? token)
    {
        token = Peek();
        return Match(type);
    }

    private bool Match(TokenType[] types, out Token? token) 
    {
        token = Peek();
        return Match(types);
    }

    public List<Node> Program()
    {
        var statements = Statements();

        return statements;
    }

    private List<Node> Statements()
    {
        var statements = new List<Node>();
        while (!Match(TokenType.EOF))
            statements.Add(Statement());

        return statements;
    }

    private Node Statement() =>
        Peek().Type switch
        {
            TokenType.Identifier when Peek(1).Type is TokenType.String or TokenType.Int or TokenType.Bool or TokenType.Float => VariableDeclaration(),
            TokenType.Identifier when Peek(1).Type is TokenType.Assign => AssignmentStatement(),
            TokenType.Identifier => throw new Exception("treat here, could be either declaration or assignment."),

            TokenType.Input => InputStatement(),
            TokenType.Output => OutputStatement(),

            TokenType.If => IfStatement(),

            TokenType.EOF => throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedEOF),

            TokenType.Comma or TokenType.Colon => 
                throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedCharacter, "this punctuation mark was not supposed to be here."),
            
            _ => throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedToken, "Expected a statement.")
        };

    private Node InputStatement()
    {
        Match(TokenType.Input, out var start);

        if (!Match(TokenType.Identifier, out var identifier))
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.ExpectedKeyword, "a variable/identifier");

        return new InputStatementNode(start!, new VariableNode(identifier!));
    }

    private Node OutputStatement()
    {
        Match(TokenType.Output, out var start);
        var values = CommaSeparatedExpressions();

        return new OutputStatementNode(start!, values);
    }

    private List<Node> CommaSeparatedExpressions()
    {
        List<Node> expressions = [Expression()];

        while (Match(TokenType.Comma))
        {
            try
            {
                expressions.Add(Expression());
            }
            catch (CompilerException ex) when (ex.Type is CompilerExceptionType.MissingExpression)
            {
                throw new CompilerException(ex.Line, ex.Column, CompilerExceptionType.MissingValueAfterComma);
            }
        }

        return expressions;
    }

    private VariableDeclarationNode VariableDeclaration()
    {
        Match(TokenType.Identifier, out var identifier);

        if (!Match([TokenType.Int, TokenType.Float, TokenType.String, TokenType.Bool], out var type))
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "The provided variable type is invalid. Allowed values are: int, float, string, or bool.");

        if (!Match(TokenType.Assign))
            return new VariableDeclarationNode(identifier!, type!.Type, identifier!.Lexeme!);

        Node value;
        try
        {
            value = Expression();
        }
        catch (CompilerException)
        {
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "To initialize a variable, you must provide either a number, string or an identifier.");
        }

        return new VariableDeclarationNode(identifier!, type!.Type, identifier!.Lexeme!, value);
    }

    private AssignmentStatementNode AssignmentStatement()
    {
        Match(TokenType.Identifier, out var identifier);
        Match(TokenType.Assign);

        Node value;
        try
        {
            value = Expression();
        }
        catch (CompilerException)
        {
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidAssignmentValue, "Invalid value.");
        }

        return new AssignmentStatementNode(identifier!, value);
    }

    private IfStatementNode IfStatement()
    {
        Match(TokenType.If, out var start);

        Node condition;
        try 
        {
            condition = Expression();
        }
        catch
        {
            throw new Exception("expected a condition...");
        }

        if (!Match(TokenType.Colon))
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedCharacter, "The condition of an if statement must be followed by a colon.");

        List<Node> block = [];
        while (!Match([TokenType.EndIf, TokenType.Else]))
        {
            try
            {
                block.Add(Statement());
            }
            catch (CompilerException ex) when (ex.Type == CompilerExceptionType.UnexpectedEOF)
            {
                throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedEOF, "You must finish your if block with a 'endif' keyword.");
            }
        }

        // IfStatementNode? elseNode = null;
        // if (Match(TokenType.Else))
        //     elseNode = ElseStatement();

        Match(TokenType.EndIf); // endif
        return new IfStatementNode(start!, condition, block);
    }

    // private void ElseIfStatement()
    // {

    // }

    // private IfStatementNode ElseStatement()
    // {
    //     if (!Match(TokenType.Else))
    //         throw new Exception("internal error -- shouldnt be called...");

    //     if (Match())
    // }

    private Node Expression()
    {
        return Comparison();
    }

    // private void ComparisonOperator()
    // {

    // }

    private Node Comparison()
    {
        var left = Additive();
        while (Match([TokenType.GreaterThan, TokenType.GreaterThanOrEqual, TokenType.LessThan, TokenType.LessThanOrEqual, TokenType.EqualsTo, TokenType.NotEqualsTo], out var op))
        {
            var right = Expression();
            return new BinaryExpressionNode(left, op!.Type, right);
        }

        return left;
    }

    private Node Additive()
    {
        var left = Multiplicative();
        while (Match([TokenType.Subtract, TokenType.Add], out var op))
        {
            var right = Expression();
            return new BinaryExpressionNode(left, op!.Type, right);
        }

        return left;
    }

    private Node Multiplicative()
    {
        var left = Primary();
        while (Match([TokenType.Multiply, TokenType.Divide], out var op))
        {
            var right = Expression();
            return new BinaryExpressionNode(left, op!.Type, right);
        }

        return left;
    }

    private Node Primary()
    {
        if (Match([TokenType.NumberLiteral, TokenType.StringLiteral, TokenType.True, TokenType.False], out var op))
            return new LiteralNode(op!);

        if (Match(TokenType.Identifier, out var identifier))
            return new VariableNode(identifier!);

        if (Match(TokenType.LeftParenthesis, out var start))
        {
            Node expr;
            try
            {
                expr = Expression();
            }
            catch (CompilerException ex) when (ex.Type == CompilerExceptionType.UnexpectedCharacter)
            {
                throw new CompilerException(ex.Line, ex.Column, CompilerExceptionType.UnclosedParenthesis);
            }
            
            if (!Match(TokenType.RightParenthesis))
                throw new CompilerException(start!.Line, start.Column, CompilerExceptionType.UnclosedParenthesis, Peek());

            return new GroupingNode(expr);
        }

        if (Match([TokenType.Subtract], out var token))
            return new UnaryExpressionNode(token!, token!.Type, Expression());

        throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.MissingExpression, "Missing expression.");
    }
}
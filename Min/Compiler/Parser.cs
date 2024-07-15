using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal class Parser
{
    private IEnumerable<Token> _tokens { get; init; }
    private int _current = 0;
    
    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
    }

    private Token Peek(int positions = 0) => _tokens.ElementAtOrDefault(_current + positions) ?? _tokens.Last();

    public bool Match(TokenType type) => Peek().Type == type;
    public bool Match(params TokenType[] types) => types.Contains(Peek().Type);

    public void Advance() => _current++;

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
            TokenType.Identifier when Peek(1).Type is TokenType.Colon => VariableDeclaration(),
            TokenType.Identifier when Peek(1).Type is TokenType.Assign => AssignmentStatement(),
            TokenType.Identifier => throw new Exception("treat here, could be either declaration or assignment."),

            TokenType.Input => InputStatement(),
            TokenType.Output => OutputStatement(),

            TokenType.If => IfStatement(),

            TokenType.EOF => throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.UnexpectedEOF),
            _ => throw new Exception("treat here...")
        };

        // var startToken = Peek();
        // var type = startToken.Type;

        // if (type is TokenType.Identifier)
        // {
        //     Advance();

        //     if (!Match(TokenType.Colon))
        //         throw new CompilerException(startToken.Line, startToken.Column, CompilerExceptionType.InvalidVariableDeclaration, "Variable declarations must specify their types.");

        //     Advance();
        //     if (!Match(TokenType.Int, TokenType.Float, TokenType.String, TokenType.Bool))
        //         throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "The provided variable type is invalid. Allowed values are: int, float, string, or bool.");

        //     var variableType = Peek().Type;
        //     Advance();
            
        //     if (!Match(TokenType.Assign))
        //         return new VariableDeclarationNode(startToken, variableType, startToken.Lexeme!);
            
        //     Advance();
        //     Node value;
        //     try
        //     {
        //         value = Expression();
        //     }
        //     catch (CompilerException)
        //     {
        //         throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "To intiialize a variable, you must provide either a number, string or an identifier.");
        //     }

        //     return new VariableDeclarationNode(startToken, variableType, startToken.Lexeme!, value);
        // }

        // throw new NotImplementedException();



    private Node InputStatement()
    {
        var start = Peek();
        Advance();

        var values = CommaSeparatedExpressions();

        return new InputStatementNode(start, values);
    }

    private Node OutputStatement()
    {
        var start = Peek();
        Advance();

        var values = CommaSeparatedExpressions();

        return new OutputStatementNode(start, values);
    }

    private List<Node> CommaSeparatedExpressions()
    {
        List<Node> expressions = [Expression()];

        while (Match(TokenType.Comma))
        {
            Advance();
            expressions.Add(Expression());
        }

        return expressions;
    }

    private VariableDeclarationNode VariableDeclaration()
    {
        var identifier = Peek();
        Advance();
        Advance(); // :

        if (!Match(TokenType.Int, TokenType.Float, TokenType.String, TokenType.Bool))
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "The provided variable type is invalid. Allowed values are: int, float, string, or bool.");

        var type = Peek().Type;
        Advance();

        if (!Match(TokenType.Assign))
            return new VariableDeclarationNode(identifier, type, identifier.Lexeme!);

        Advance();
        
        Node value;
        try
        {
            value = Expression();
        }
        catch (CompilerException)
        {
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "To intiialize a variable, you must provide either a number, string or an identifier.");
        }

        return new VariableDeclarationNode(identifier, type, identifier.Lexeme!, value);
    }

    private AssignmentStatementNode AssignmentStatement()
    {
        var identifier = Peek();
        Advance();
        Advance(); // =

        Node value;
        try
        {
            value = Expression();
        }
        catch (CompilerException)
        {
            throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidVariableDeclaration, "To intiialize a variable, you must provide either a number, string or an identifier.");
        }

        return new AssignmentStatementNode(identifier, value);
    }

    private IfStatementNode IfStatement()
    {
        var start = Peek();
        Advance();

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

        Advance();

        List<Node> block = [];
        while (!Match(TokenType.EndIf, TokenType.Else))
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

        Advance(); // endif
        return new IfStatementNode(start, condition, block);
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
        while (Match(TokenType.GreaterThan, TokenType.GreaterThanOrEqual, TokenType.LessThan, TokenType.LessThanOrEqual, TokenType.EqualsTo, TokenType.NotEqualsTo))
        {
            var op = Peek();
            Advance();

            var right = Expression();
            return new BinaryExpressionNode(left, op.Type, right);
        }

        return left;
    }

    private Node Additive()
    {
        var left = Multiplicative();
        while (Match(TokenType.Subtract, TokenType.Add))
        {
            var op = Peek();
            Advance();

            var right = Expression();
            return new BinaryExpressionNode(left, op.Type, right);
        }

        return left;
    }

    private Node Multiplicative()
    {
        var left = Primary();
        while (Match(TokenType.Multiply, TokenType.Divide))
        {
            var op = Peek();
            Advance();

            var right = Expression();
            return new BinaryExpressionNode(left, op.Type, right);
        }

        return left;
    }

    private Node Primary()
    {
        if (Match(TokenType.NumberLiteral, TokenType.StringLiteral, TokenType.True, TokenType.False))
        {
            var token = Peek();
            Advance();

            return new LiteralNode(token);
        }

        if (Match(TokenType.Identifier))
        {
            var token = Peek();
            Advance();

            return new VariableNode(token);
        }

        // @todo punctuation are not implemented in the tokenizer...
        if (Match(TokenType.LeftParenthesis))
        {
            var start = Peek();

            Advance();
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
                throw new CompilerException(start.Line, start.Column, CompilerExceptionType.UnclosedParenthesis, Peek());

            Advance();
            return new GroupingNode(expr);
        }

        throw new CompilerException(Peek().Line, Peek().Column, CompilerExceptionType.InvalidExpression, "An expression can only contain numbers, strings or identifiers.");
    }
}
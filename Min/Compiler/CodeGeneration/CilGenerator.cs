using System.Text;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public class CilGenerator : ICodeGenerator, IVisitor<string>
{
    private readonly SymbolTable _symbols;
    private readonly TypeChecker _typeChecker;

    public CilGenerator()
    {
        _symbols = new();
        _typeChecker = new(_symbols);
    }

    public string Generate(List<Node> nodes)
    {
        var code = new StringBuilder();

        foreach (var node in nodes)
            code.AppendLine(node.Accept(this));

        return code.ToString();        
    }

    public string Visit(VariableDeclarationNode node)
    {
        try
        {
            _symbols.Add(node.Name, node.VariableType);
        }
        catch (InternalCompilerException ex) when (ex.Type is CompilerExceptionType.IdentifierAlreadyDeclared)
        {
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierAlreadyDeclared, node.Name);
        }

        var value = node.Value is not null ? node.Value.Accept(this) : "";
        if (node.Value is not null && !_typeChecker.Check(node.Value, node.VariableType))
            throw new CompilerException(node.Value.Start.Line, node.Value.Start.Column, CompilerExceptionType.IncompatibleType, $"{node.Name} expects a {node.VariableType}.");

        return $"{node.Name} {{ {value} }}";
    }

    public string Visit(LiteralNode node) =>
        node.Start.Type switch
        {
            TokenType.NumberLiteral => $"number {node.Token.Lexeme}",
            TokenType.StringLiteral => $"string \"{node.Token.Lexeme}\"",
            TokenType.True or TokenType.False => $"bool {node.Token.Lexeme}",
            _ => throw new Exception()
        };

    public string Visit(BinaryExpressionNode node)
    {
        var message = $"{node.Operator} {{ {node.Left.Accept(this)}, {node.Right.Accept(this)} }}";

        return message;
    }

    public string Visit(VariableNode node)
    {
        if (!_symbols.IsSaved(node.Name))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Name);

        return node.Name;
    }

    public string Visit(GroupingNode node)
    {
        return $"( {node.Expression.Accept(this)} )";
    }

    public string Visit(AssignmentStatementNode node)
    {
        return $"assign {{ {node.Identifier} = {node.Value.Accept(this)} }}";
    }

    public string Visit(InputStatementNode node)
    {
        return $"input {{ {node.Variable.Accept(this)} }}";
    }

    public string Visit(OutputStatementNode node)
    {
        return "output { " + string.Join(", ", node.Values.Select(node => node.Accept(this))) + " }";
    }

    public string Visit(IfStatementNode node)
    {
        return $"if ({node.Condition!.Accept(this)}) {{ {string.Join(", ", node.Block.Select(n => n.Accept(this)))} }}";
    }
}
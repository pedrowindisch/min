using System.Text;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public class CilGenerator(SymbolTable symbols, List<Node> nodes) 
    : BaseCodeGenerator(symbols, nodes), IVisitor<string>
{
    public override string Execute()
    {
        var code = new StringBuilder();

        foreach (var node in _nodes)
            code.AppendLine(node.Accept(this));

        return code.ToString();        
    }

    public string Visit(VariableDeclarationNode node)
    {
        var value = node.Value is not null ? node.Value.Accept(this) : "";

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

    public string Visit(UnaryExpressionNode node)
    {
        throw new NotImplementedException();
    }
}
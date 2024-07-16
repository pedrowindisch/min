using System.Text;
using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration.Cil;

public class CilGenerator : ICodeGenerator, IVisitor<string>
{
    public string Generate(List<Node> nodes)
    {
        var code = new StringBuilder();

        foreach (var node in nodes)
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
            TokenType.NumberLiteral => $"number {node.Value}",
            TokenType.StringLiteral => $"string \"{node.Value}\"",
            TokenType.True or TokenType.False => $"bool {node.Value}",
            _ => throw new Exception()
        };

    public string Visit(BinaryExpressionNode node)
    {
        return $"{node.Operator} {{ {node.Left}, {node.Right} }}";
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
}
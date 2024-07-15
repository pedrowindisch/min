using System.Text;
using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration.Cil;

internal class CilGenerator : ICodeGenerator, IVisitor<string>
{
    private List<Node> _nodes { get; init; }

    public CilGenerator(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public string Generate()
    {
        var code = new StringBuilder();

        foreach (var node in _nodes)
            code.AppendLine(node.Accept(this));

        return code.ToString();        
    }

    public string Visit(VariableDeclarationNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(LiteralNode node) =>
        node.Start.Type switch
        {
            TokenType.NumberLiteral => $"number {node.Value}",
            _ => throw new Exception()
        };

    public string Visit(BinaryExpressionNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(VariableNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(GroupingNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(AssignmentStatementNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(InputStatementNode node)
    {
        throw new NotImplementedException();
    }

    public string Visit(OutputStatementNode node)
    {
        return "output { " + string.Join(", ", node.Values.Select(node => node.Accept(this))) + " }";
    }

    public string Visit(IfStatementNode node)
    {
        throw new NotImplementedException();
    }
}
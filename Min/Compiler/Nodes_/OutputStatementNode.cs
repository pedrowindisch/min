namespace Min.Compiler.Nodes2;

public class OutputStatementNode : StatementNode
{
    public List<ExpressionNode> Values { get; init; }

    public OutputStatementNode(Position position, List<ExpressionNode> values) : base(position)
    {
        Values = values;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
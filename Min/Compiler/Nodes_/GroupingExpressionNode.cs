namespace Min.Compiler.Nodes2;

public class GroupingExpressionNode : ExpressionNode
{
    public ExpressionNode Value { get; init; }

    public GroupingExpressionNode(Position start, ExpressionNode value) : base(start)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

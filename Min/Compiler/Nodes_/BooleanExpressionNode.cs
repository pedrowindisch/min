namespace Min.Compiler.Nodes2;

public class BooleanExpressionNode : ExpressionNode
{
    public bool Value { get; init; }

    public BooleanExpressionNode(Position start, bool value) : base(start)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

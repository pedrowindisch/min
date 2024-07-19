namespace Min.Compiler.Nodes2;

public class NumberExpressionNode : ExpressionNode
{
    public double Value { get; init; }

    public NumberExpressionNode(Position start, double value) : base(start)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

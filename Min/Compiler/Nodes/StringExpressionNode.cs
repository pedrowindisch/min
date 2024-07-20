namespace Min.Compiler.Nodes;

public class StringExpressionNode : ExpressionNode
{
    public string Value { get; init; }

    public StringExpressionNode(Position start, string value) : base(start)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
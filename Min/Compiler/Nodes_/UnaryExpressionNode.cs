namespace Min.Compiler.Nodes2;

public class UnaryExpressionNode : ExpressionNode
{
    public BuiltInOperator Operator { get; set; }
    public ExpressionNode Value { get; init; }

    public UnaryExpressionNode(Position start, BuiltInOperator op, ExpressionNode value) : base(start)
    {
        Operator = op;
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

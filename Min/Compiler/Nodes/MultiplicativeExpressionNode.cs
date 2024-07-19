namespace Min.Compiler.Nodes;

public class MultiplicativeExpressionNode : ExpressionNode
{
    public ExpressionNode Left { get; init; }
    public BuiltInOperator Operator { get; init; }
    public ExpressionNode Right { get; init; }

    public MultiplicativeExpressionNode(Position start, ExpressionNode left, BuiltInOperator op, ExpressionNode right) : base(start)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

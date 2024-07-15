namespace Min.Compiler.Nodes;

internal class BinaryExpressionNode : Node
{
    public Node Left { get; init; }
    public Node Right { get; init; }
    public TokenType Operator { get; init; }

    public BinaryExpressionNode(Node left, TokenType op, Node right) : base(left.Start)
    {
        Left = left;
        Right = right;
        Operator = op;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

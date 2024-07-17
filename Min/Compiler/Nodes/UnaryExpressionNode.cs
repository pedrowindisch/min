namespace Min.Compiler.Nodes;

public class UnaryExpressionNode : Node
{
    public TokenType Operator { get; init; }
    public Node Expression { get; init; }

    public UnaryExpressionNode(Token opToken, TokenType op, Node expression) : base(opToken)
    {
        Operator = op;
        Expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

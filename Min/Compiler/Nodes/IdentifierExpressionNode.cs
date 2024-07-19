namespace Min.Compiler.Nodes;

public class IdentifierExpressionNode : ExpressionNode
{
    public string Identifier { get; init; }

    public IdentifierExpressionNode(Position start, string identifier) : base(start)
    {
        Identifier = identifier;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

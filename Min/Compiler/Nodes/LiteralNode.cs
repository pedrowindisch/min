namespace Min.Compiler.Nodes;

public class LiteralNode : Node
{
    public Token Token { get; init; }

    public LiteralNode(Token token) : base(token)
    {
        Token = token;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
namespace Min.Compiler.Nodes;

internal class LiteralNode : Node
{
    public string Value { get; init; }

    public LiteralNode(Token token) : base(token)
    {
        Value = token.Lexeme!;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
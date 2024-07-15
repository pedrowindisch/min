namespace Min.Compiler.Nodes;

internal class InputStatementNode : Node
{
    public List<Node> Values { get; set; }

    public InputStatementNode(Token start, List<Node> values) : base(start)
    {
        Values = values;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

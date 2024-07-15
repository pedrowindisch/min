namespace Min.Compiler.Nodes;

internal class OutputStatementNode : Node
{
    public List<Node> Values { get; set; }

    public OutputStatementNode(Token start, List<Node> values) : base(start)
    {
        Values = values;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

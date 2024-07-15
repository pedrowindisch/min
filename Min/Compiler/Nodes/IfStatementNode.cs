namespace Min.Compiler.Nodes;

internal class IfStatementNode : Node
{
    public Node Condition { get; init; }
    public List<Node> Block { get; init; }
    public IfStatementNode? Else { get; init; }

    public IfStatementNode(Token start, Node condition, List<Node> block) : base(start)
    {
        Condition = condition;
        Block = block;
    }

    public IfStatementNode(Token start, Node condition, List<Node> block, IfStatementNode? elseNode) : this(start, condition, block)
    {
        Else = elseNode;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

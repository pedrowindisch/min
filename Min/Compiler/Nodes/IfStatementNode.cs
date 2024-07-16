namespace Min.Compiler.Nodes;

public class IfStatementNode : Node
{
    public Node? Condition { get; init; }
    public List<Node> Block { get; init; }
    public IfStatementNode? Else { get; init; }

    public IfStatementNode(Token start, List<Node> block) : base(start)
    {
        Block = block;
    }

    public IfStatementNode(Token start, Node condition, List<Node> block) : this(start, block)
    {
        Condition = condition;
    }

    public IfStatementNode(Token start, Node condition, List<Node> block, IfStatementNode? elseNode) : this(start, condition, block)
    {
        Else = elseNode;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

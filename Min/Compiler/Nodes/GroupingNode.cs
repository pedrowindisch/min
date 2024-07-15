namespace Min.Compiler.Nodes;

internal class GroupingNode : Node
{
    public Node Expression { get; init; }

    public GroupingNode(Node expression) : base(expression.Start)
    {
        Expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
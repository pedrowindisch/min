namespace Min.Compiler.Nodes;

public class IfStatementNode : StatementNode
{
    public ExpressionNode Condition { get; init; }
    public List<StatementNode> Block { get; init; }

    public IfStatementNode(Position start, ExpressionNode condition, List<StatementNode> block) : base(start)
    {
        Condition = condition;
        Block = block;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

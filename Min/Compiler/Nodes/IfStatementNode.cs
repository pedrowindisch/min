namespace Min.Compiler.Nodes;

public class IfStatementNode : StatementNode
{
    public ExpressionNode Condition { get; init; }
    public List<StatementNode> Block { get; init; }

    public List<ElseIfStatementNode>? ElseIfStatements { get; init; } = null;
    public ElseStatementNode? ElseStatement { get; init; } = null;

    public IfStatementNode(Position start, ExpressionNode condition, List<StatementNode> block, List<ElseIfStatementNode>? elseIfStatements = null, ElseStatementNode? elseStatement = null) : base(start)
    {
        Condition = condition;
        Block = block;
        ElseIfStatements = elseIfStatements;
        ElseStatement = elseStatement;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

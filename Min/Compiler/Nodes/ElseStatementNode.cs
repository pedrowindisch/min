namespace Min.Compiler.Nodes;

public class ElseStatementNode : StatementNode
{
    public List<StatementNode> Statements { get; init; }

    public ElseStatementNode(Position start, List<StatementNode> statements) : base(start)
    {
        Statements = statements;
    } 

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
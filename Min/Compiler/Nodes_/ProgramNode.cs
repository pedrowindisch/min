namespace Min.Compiler.Nodes2;

public class ProgramNode : ASTNode
{
    public List<StatementNode> Statements { get; init; }

    public ProgramNode(List<StatementNode> statements) : base(statements.ElementAt(0).Start)
    {
        Statements = statements;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
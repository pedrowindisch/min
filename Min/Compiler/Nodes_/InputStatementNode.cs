namespace Min.Compiler.Nodes2;

public class InputStatementNode : StatementNode
{
    public string Identifier { get; init; }

    public InputStatementNode(Position start, string identifier) : base(start)
    {
        Identifier = identifier;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
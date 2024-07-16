namespace Min.Compiler.Nodes;

public class AssignmentStatementNode : Node
{
    public string Identifier { get; init; }
    public Node Value { get; init; }

    public AssignmentStatementNode(Token identifier, Node value) : base(identifier)
    {
        Identifier = identifier.Lexeme!;
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

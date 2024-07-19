namespace Min.Compiler.Nodes2;

public class VariableAssignmentnNode : StatementNode
{
    public string Identifier { get; init; }
    public ExpressionNode Value { get; init; }

    public VariableAssignmentnNode(Position position, string identifier, ExpressionNode value) : base(position)
    {
        Identifier = identifier;
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
namespace Min.Compiler.Nodes;

public class VariableAssignmentNode : StatementNode
{
    public string Identifier { get; init; }
    public ExpressionNode Value { get; init; }

    public VariableAssignmentNode(Position position, string identifier, ExpressionNode value) : base(position)
    {
        Identifier = identifier;
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
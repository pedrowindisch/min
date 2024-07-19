namespace Min.Compiler.Nodes;

public class VariableDeclarationNode : StatementNode
{
    public string Identifier { get; init; }
    public BuiltInType Type { get; init; }
    public ExpressionNode? Value { get; init; }

    public VariableDeclarationNode(Position position, string identifier, BuiltInType type) : base(position)
    {
        Identifier = identifier;
        Type = type;
    }

    public VariableDeclarationNode(Position position, string identifier, BuiltInType type, ExpressionNode value) : this(position, identifier, type)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}
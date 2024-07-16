using Min.Compiler.Exceptions;

namespace Min.Compiler.Nodes;

public class VariableDeclarationNode : Node
{
    public TokenType VariableType { get; init; }
    public string Name { get; init; }
    public Node? Value { get; init; }

    public VariableDeclarationNode(Token start, TokenType variableType, string name) : base(start)
    {
        VariableType = variableType;
        Name = name;
    }

    public VariableDeclarationNode(Token start, TokenType variableType, string name, Node? value) : this(start, variableType, name)
    {
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
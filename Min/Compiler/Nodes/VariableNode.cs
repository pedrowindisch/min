namespace Min.Compiler.Nodes;

internal class VariableNode : Node
{
    public string Name { get; init; }

    public VariableNode(Token identifierToken) : base(identifierToken)
    {
        Name = identifierToken.Lexeme!;
    } 

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
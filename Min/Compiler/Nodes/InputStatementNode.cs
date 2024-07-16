namespace Min.Compiler.Nodes;

public class InputStatementNode : Node
{
    public VariableNode Variable { get; set; }

    public InputStatementNode(Token start, VariableNode variable) : base(start)
    {
        Variable = variable;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

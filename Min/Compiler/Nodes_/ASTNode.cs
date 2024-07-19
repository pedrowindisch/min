namespace Min.Compiler.Nodes2;

public abstract class ASTNode
{
    public Position Start { get; init; }
    public Position? End { get; init; }

    protected ASTNode(Position start)
    {
        Start = start;
    }

    protected ASTNode(Position start, Position end) : this(start)
    {
        End = end;
    }

    public abstract void Accept(IVisitor visitor);
}
namespace Min.Compiler.Nodes;

public abstract class Node(Token start)
{
    public Token Start { get; init; } = start;

    public abstract T Accept<T>(IVisitor<T> visitor);
}
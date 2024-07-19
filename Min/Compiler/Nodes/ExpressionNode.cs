namespace Min.Compiler.Nodes;

public abstract class ExpressionNode : ASTNode
{
    protected ExpressionNode(Position start) : base(start) { }
    protected ExpressionNode(Position start, Position end) : base(start, end) { }
}

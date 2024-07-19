namespace Min.Compiler.Nodes2;

public abstract class ExpressionNode : ASTNode
{
    protected ExpressionNode(Position start) : base(start) { }
    protected ExpressionNode(Position start, Position end) : base(start, end) { }
}

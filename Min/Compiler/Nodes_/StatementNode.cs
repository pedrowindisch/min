using Min.Compiler.Nodes2;

namespace Min.Compiler;

public abstract class StatementNode : ASTNode
{
    protected StatementNode(Position start) : base(start) { }
    protected StatementNode(Position start, Position end) : base(start, end) { }
} 
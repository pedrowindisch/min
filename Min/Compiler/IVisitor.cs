using Min.Compiler.Nodes;

namespace Min.Compiler;

public interface IVisitor<T>
{
    T Visit(VariableDeclarationNode node);
    T Visit(LiteralNode node);
    T Visit(BinaryExpressionNode node);
    T Visit(VariableNode node);
    T Visit(GroupingNode node);
    T Visit(AssignmentStatementNode node);
    T Visit(InputStatementNode node);
    T Visit(OutputStatementNode node);
    T Visit(IfStatementNode node);
}
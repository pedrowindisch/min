using Min.Compiler.Nodes;

namespace Min.Compiler;

public interface IVisitor
{
    void Visit(ProgramNode node);
    void Visit(VariableDeclarationNode node);
    void Visit(VariableAssignmentNode node);
    void Visit(IfStatementNode node);
    void Visit(InputStatementNode node);
    void Visit(OutputStatementNode node);
    void Visit(ComparisonExpressionNode node);
    void Visit(MultiplicativeExpressionNode node);
    void Visit(AdditiveExpressionNode node);
    void Visit(UnaryExpressionNode node);
    void Visit(NumberExpressionNode node);
    void Visit(BooleanExpressionNode node);
    void Visit(IdentifierExpressionNode node);
    void Visit(GroupingExpressionNode node);
}
namespace Min.Compiler.Nodes;

public class ElseIfStatementNode(
    Position start,
    ExpressionNode condition,
    List<StatementNode> statements
) : IfStatementNode(start, condition, statements) { }
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal sealed class TypeChecker : ISemanticAnalysisStep, IVisitor
{
    private SymbolTable _symbols = null!;

    public (SymbolTable, ProgramNode) Execute(SymbolTable symbols, ProgramNode nodes)
    {
        _symbols = symbols;
        foreach (var node in nodes.Statements)
            node.Accept(this);

        return (_symbols, nodes);
    }

    private BuiltInType CheckType(ExpressionNode expr)
    {
        if (expr is StringExpressionNode) return BuiltInType.String;
        if (expr is BooleanExpressionNode) return BuiltInType.Bool;
        if (expr is NumberExpressionNode) return BuiltInType.Int;
        if (expr is IdentifierExpressionNode vex) return _symbols.GetType(vex.Identifier);
        if (expr is GroupingExpressionNode gex) return CheckType(gex.Value);
        if (expr is UnaryExpressionNode uex) return CheckType(uex.Value);

        if (expr is ComparisonExpressionNode cex)
        {
            var leftType = CheckType(cex.Left);
            var rightType = CheckType(cex.Right);

            if (leftType != rightType)
                throw new Exception("Operations only work on equal types.");

            if (cex.Operator is BuiltInOperator.GreaterThan or BuiltInOperator.GreaterThanOrEquals or BuiltInOperator.LessThan or BuiltInOperator.LessThanOrEquals)
            {
                if (leftType is not (BuiltInType.Int or BuiltInType.Float))
                    throw new Exception("Relational operators only works on integers");
            }

            return BuiltInType.Bool;
        }

        if (expr is AdditiveExpressionNode aex)
        {
            var leftType = CheckType(aex.Left);
            var rightType = CheckType(aex.Right);

            if (leftType != rightType)
                throw new Exception("Operations only work on equal types.");

            if (aex.Operator is BuiltInOperator.Subtract)
            {
                if (leftType is not (BuiltInType.Int or BuiltInType.Float))
                    throw new Exception("Subtraction only works on integers");

                return leftType;
            }

            if (leftType is BuiltInType.Bool)
                throw new Exception("Addition does not work on bools");

            return leftType;
        }


        if (expr is MultiplicativeExpressionNode mex)
        {
            var leftType = CheckType(mex.Left);
            var rightType = CheckType(mex.Right);

            if (leftType != rightType)
                throw new Exception("Operations only work on equal types.");

            if (mex.Operator is BuiltInOperator.Subtract)
            {
                if (leftType is not (BuiltInType.Int or BuiltInType.Float))
                    throw new Exception("Subtraction only works on integers");
            }

            if (leftType is BuiltInType.Bool)
                throw new Exception("Addition does not work on bools");

            return leftType;
        }

        throw new Exception("not implemented");
    }

    private bool IsExpectedType(BuiltInType expected, ExpressionNode expr) => expected == CheckType(expr);
    private bool IsExpectedType(BuiltInType[] expected, ExpressionNode expr) => expected.Contains(CheckType(expr));

    public void Visit(UnaryExpressionNode node)
    {
        if (!IsExpectedType([BuiltInType.Int, BuiltInType.Float], node.Value))
            throw new Exception("unary expression should only accept integers or floats");
    }
    
    public void Visit(VariableAssignmentNode node)
    {
        if (!IsExpectedType(_symbols.GetType(node.Identifier), node.Value))
            throw new Exception("wrong value - diffent type than the variable declaration");
    }

    public void Visit(IfStatementNode node)
    {
        if (!IsExpectedType(BuiltInType.Bool, node.Condition))
            throw new Exception("wrong condition type");    
    }

    public void Visit(VariableDeclarationNode node)
    {
        if (node.Value is null) return;

        if (!IsExpectedType(node.Type, node.Value))
            throw new Exception("wrong declaration type");
    }

    public void Visit(OutputStatementNode node) 
    {
        foreach (var value in node.Values) 
            value.Accept(this);
    }

    public void Visit(ComparisonExpressionNode node) => CheckType(node);
    public void Visit(MultiplicativeExpressionNode node) => CheckType(node);
    public void Visit(AdditiveExpressionNode node) => CheckType(node);
    public void Visit(GroupingExpressionNode node) => CheckType(node);
    
    public void Visit(ProgramNode node) { }
    public void Visit(InputStatementNode node) { }
    public void Visit(StringExpressionNode node) { }
    public void Visit(NumberExpressionNode node) { }
    public void Visit(BooleanExpressionNode node) { }
    public void Visit(IdentifierExpressionNode node) { }
}

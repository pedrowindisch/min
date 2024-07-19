using Min.Compiler.Nodes;

namespace Min.Compiler;

internal sealed class TypeChecker : ISemanticAnalysisStep // , IVisitor<TokenType>
{
    private SymbolTable _symbols = null!;

    public (SymbolTable, ProgramNode) Execute(SymbolTable symbols, ProgramNode nodes)
    {
        // _symbols = symbols;
        // foreach (var node in nodes)
        //     node.Accept(this);

        return (_symbols, nodes);
    }

    // private TokenType BinaryOperationToExpectedType(TokenType op, TokenType leftSideType)
    // {
    //     // Equality operators - both sides must be the same.
    //     // if (op is TokenType.EqualsTo or TokenType.NotEqualsTo) 
    //     //     return leftSideType;

    //     // if (op is TokenType.GreaterThanOrEqual or TokenType.GreaterThan or TokenType.LessThanOrEqual or TokenType.LessThan)
    //     // {
    //     //     if (leftSideType is not TokenType.Int or TokenType.Float)
    //     //         throw new Exception("invalid types - relational operators only work on numeric types");

    //     //     return leftSideType;
    //     // }

    //     // if (leftSideType is TokenType.Bool)
    //     //     throw new Exception("booleans do not support binary operations.");

    //     // if (leftSideType is TokenType.String)
    //     // {
    //     //     if (op is TokenType.Divide or TokenType.Subtract)
    //     //         throw new Exception("cannot divide or subtract strings");

    //     //     return TokenType.String;
    //     // }

    //     // return leftSideType;
    // }

    // public TokenType Visit(VariableDeclarationNode node)
    // {
    //     // if (node.Value is not null)
    //     // {
    //     //     var valueType = node.Value.Accept(this);
    //     //     if (valueType != node.VariableType)
    //     //         throw new Exception("invalid declaration - types are not compatible");
    //     // }

    //     // return node.VariableType;
    // }

    // public TokenType Visit(LiteralNode node) =>
    //     node.Token.Type switch
    //     {
    //         TokenType.NumberLiteral => TokenType.Int, 
    //         TokenType.StringLiteral => TokenType.String, 
    //         TokenType.True => TokenType.Bool, 
    //         TokenType.False => TokenType.Bool, 
    //     };

    // public TokenType Visit(BinaryExpressionNode node)
    // {
    //     var leftSideType = node.Left.Accept(this);
    //     var rightSideType = node.Right.Accept(this);

    //     var expectedType = BinaryOperationToExpectedType(node.Operator, leftSideType);
    //     if (rightSideType != expectedType)
    //         throw new Exception("invalid operation - types are not compatible");

    //     if (node.Operator is TokenType.EqualsTo or TokenType.NotEqualsTo or TokenType.GreaterThan or TokenType.GreaterThanOrEqual or TokenType.LessThan or TokenType.LessThanOrEqual)
    //         return TokenType.Bool;

    //     return expectedType;
    // }

    // public TokenType Visit(UnaryExpressionNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(VariableNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(GroupingNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(AssignmentStatementNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(InputStatementNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(OutputStatementNode node)
    // {
    //     return default;
    // }

    // public TokenType Visit(IfStatementNode node)
    // {
    //     if (node.Condition is not null)
    //     {
    //         if (node.Condition.Accept(this) is not TokenType.Bool)
    //             throw new Exception("invalid type...");
    //     }

    //     return default;
    // }
}

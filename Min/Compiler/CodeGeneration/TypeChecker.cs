using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

internal class TypeChecker(SymbolTable symbols)
{
    private readonly SymbolTable _symbols = symbols;

    public bool Check(Node node, TokenType expected)
    {
        if (expected is not (TokenType.Int or TokenType.String or TokenType.Float or TokenType.Bool))
            throw new Exception("should not be called...");

        if (node is BinaryExpressionNode expr)
            return Check(expr.Left, expected) && Check(expr.Right, expected);

        if (node is LiteralNode literal)
            return literal.Token.Type switch
            {
                TokenType.NumberLiteral => expected is TokenType.Float or TokenType.Int,
                TokenType.StringLiteral => expected is TokenType.String,
                TokenType.True or TokenType.False => expected is TokenType.Bool,

                _ => false
            };

        if (node is VariableNode var)
            return _symbols.GetType(var.Name) == expected;

        return false;
    }
}
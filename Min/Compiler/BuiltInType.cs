using Min.Compiler.Exceptions;

namespace Min.Compiler;

public enum BuiltInType
{
    Int,
    Float,
    String,
    Bool
}

public static class BuiltInTypeHelpers
{
    public static BuiltInType From(TokenType type) =>
        type switch
        {
            TokenType.Int => BuiltInType.Int,
            TokenType.Float => BuiltInType.Float,
            TokenType.Bool => BuiltInType.Bool,
            TokenType.String => BuiltInType.String,
        
            _ => throw new Exception("should not be called")
        };
}
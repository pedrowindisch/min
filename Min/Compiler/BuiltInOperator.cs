namespace Min.Compiler;

public enum BuiltInOperator
{
    EqualsTo = TokenType.EqualsTo,
    NotEqualsTo = TokenType.NotEqualsTo,
    GreaterThan = TokenType.GreaterThan,
    GreaterThanOrEquals = TokenType.GreaterThanOrEqual,
    LessThan = TokenType.LessThan,
    LessThanOrEquals = TokenType.LessThanOrEqual,    
    Add = TokenType.Add,
    Subtract = TokenType.Subtract,
    Multiply = TokenType.Multiply,
    Divide = TokenType.Divide
}
namespace Min.Compiler;

public enum TokenType
{
    Identifier,
    NumberLiteral,
    StringLiteral,
    True,
    False,

    Input,
    Output,
    If,
    Else,

    Colon,
    Comma,
    Assign,
    LeftParenthesis,
    RightParenthesis,

    Add,
    Subtract,
    Multiply,
    Divide,

    EqualsTo,
    NotEqualsTo,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,

    Int,
    Float,
    String,
    Bool,

    EOF
}
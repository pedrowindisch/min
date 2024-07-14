namespace Min.Compiler;

internal enum TokenType
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
    EndIf,

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
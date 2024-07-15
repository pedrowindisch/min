namespace Min.Compiler.Exceptions;

internal enum CompilerExceptionType
{
    UnexpectedCharacter,
    UnterminatedString,
    UnrecognizedKeyword,
    UnrecognizedOperator,
    InvalidIdentifier,
    InvalidNumberLiteral,

    UnexpectedEOF,
    InvalidVariableDeclaration,
    UnclosedParenthesis,
    EmptyExpression,
    InvalidExpression,
}

internal static class CompilerExceptionTypeExtensions
{
    public static string GenerateMessage(this CompilerExceptionType type, params object[] args) => 
        string.Format(type switch
        {
            CompilerExceptionType.UnexpectedCharacter
                when args is [char ch] => $"Expected {ch}.",
            CompilerExceptionType.UnexpectedCharacter
                when args is [string message] => message,
            CompilerExceptionType.UnexpectedCharacter => "Unexpected character: {0}",

            CompilerExceptionType.UnterminatedString => "Unterminated string",
            CompilerExceptionType.UnrecognizedKeyword => "Unrecognized keyword",
            CompilerExceptionType.UnrecognizedOperator => "Unrecognized operator: {0}",

            CompilerExceptionType.UnexpectedEOF
                when args is [string message] => message,
            CompilerExceptionType.UnexpectedEOF => "Unexpected end of file.",

            CompilerExceptionType.InvalidIdentifier 
                when args is [string message] => message,
            CompilerExceptionType.InvalidIdentifier => "Invalid identifier.",
            
            CompilerExceptionType.InvalidNumberLiteral 
                when args is [string message] => message,
            CompilerExceptionType.InvalidNumberLiteral => "Malformed number.",

            CompilerExceptionType.InvalidVariableDeclaration
                when args is [string message] => message,

            CompilerExceptionType.UnclosedParenthesis => "Unclosed parenthesis.",
            CompilerExceptionType.EmptyExpression => "Parenthesis cannot be empty.",

            CompilerExceptionType.InvalidExpression
                when args is [string message] => message,

            _ => throw new Exception("Exception type does not have a message."),
        }, args);
}
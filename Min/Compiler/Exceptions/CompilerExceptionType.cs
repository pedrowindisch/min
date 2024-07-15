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
    ExpectedKeyword,
    InvalidVariableDeclaration,
    UnclosedParenthesis,
    EmptyExpression,
    InvalidExpression,
    MissingValueAfterComma,
    MissingExpression,
    InvalidAssignmentValue
}

internal static class CompilerExceptionTypeExtensions
{
    public static string GenerateMessage(this CompilerExceptionType type, params object[] args) => 
        string.Format(type switch
        {
            CompilerExceptionType.UnexpectedCharacter
                when args is [char ch] => $"Expected {ch}.",
            _ when args is [string message] => message,
            
            CompilerExceptionType.UnexpectedCharacter => "Unexpected character: {0}",
            CompilerExceptionType.UnterminatedString => "Unterminated string",
            CompilerExceptionType.UnrecognizedKeyword => "Unrecognized keyword",
            CompilerExceptionType.UnrecognizedOperator => "Unrecognized operator: {0}",
            CompilerExceptionType.UnexpectedEOF => "Unexpected end of file.",
            CompilerExceptionType.InvalidIdentifier => "Invalid identifier.",
            CompilerExceptionType.InvalidNumberLiteral => "Malformed number.",
            CompilerExceptionType.UnclosedParenthesis => "Unclosed parenthesis.",
            CompilerExceptionType.EmptyExpression => "Parenthesis cannot be empty.",
            CompilerExceptionType.MissingValueAfterComma => "Missing value after comma.",
            CompilerExceptionType.MissingExpression => "Missing expression.",
            CompilerExceptionType.ExpectedKeyword
                when args is [string keyword] => $"Expected {keyword}.",

            _ => throw new Exception("Exception type with given arguments (or no arguments) does not have a message."),
        }, args);
}
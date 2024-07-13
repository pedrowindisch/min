namespace Min.Compiler.Exceptions;

internal enum CompilerExceptionType
{
    UnexpectedCharacter,
    UnterminatedString,
    UnrecognizedKeyword,
    UnrecognizedOperator,
    InvalidIdentifier,
}

internal static class CompilerExceptionTypeExtensions
{
    public static string GenerateMessage(this CompilerExceptionType type, params object[] args) => 
        string.Format(type switch
        {
            CompilerExceptionType.UnexpectedCharacter => "Unexpected character: {0}",
            CompilerExceptionType.UnterminatedString => "Unterminated string",
            CompilerExceptionType.UnrecognizedKeyword => "Unrecognized keyword: {0}",
            CompilerExceptionType.UnrecognizedOperator => "Unrecognized operator: {0}",
            CompilerExceptionType.InvalidIdentifier 
                when args.Length > 0 => "Invalid identifier: {0}",
            CompilerExceptionType.InvalidIdentifier => "Invalid identifier.",

            _ => throw new Exception("Exception type does not have a message."),
        }, args);
}
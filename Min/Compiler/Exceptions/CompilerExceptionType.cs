namespace Min.Compiler.Exceptions;

internal enum CompilerExceptionType
{
    UnexpectedCharacter,
    UnterminatedString,
    UnrecognizedKeyword,
    UnrecognizedOperator,
    InvalidIdentifier,
    InvalidNumberLiteral,
}

internal static class CompilerExceptionTypeExtensions
{
    public static string GenerateMessage(this CompilerExceptionType type, params object[] args) => 
        string.Format(type switch
        {
            CompilerExceptionType.UnexpectedCharacter => "Unexpected character: {0}",
            CompilerExceptionType.UnterminatedString => "Unterminated string",
            CompilerExceptionType.UnrecognizedKeyword => "Unrecognized keyword",
            CompilerExceptionType.UnrecognizedOperator => "Unrecognized operator: {0}",
            
            CompilerExceptionType.InvalidIdentifier 
                when args.Length == 1 => (string) args[0],
            CompilerExceptionType.InvalidIdentifier => "Invalid identifier.",
            
            CompilerExceptionType.InvalidNumberLiteral 
                when args.Length == 1 => (string) args[0],
            CompilerExceptionType.InvalidNumberLiteral => "Malformed number.",

            _ => throw new Exception("Exception type does not have a message."),
        }, args);
}
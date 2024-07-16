using Microsoft.Extensions.Localization;

namespace Min.Compiler.Exceptions;

public enum CompilerExceptionType
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
    InvalidAssignmentValue,
    UnexpectedToken,
}

internal static class CompilerExceptionTypeExtensions
{
    public static string GenerateMessage(this CompilerExceptionType type, params object[] args) => 
        string.Format(type switch
        {
            CompilerExceptionType.ExpectedKeyword
                when args is [string keyword] => $"Expected {keyword}.",
            CompilerExceptionType.UnrecognizedOperator
                when args is [string op] => $"Unrecognized operator: {op}",
            CompilerExceptionType.UnexpectedCharacter
                when args is [char] or [string] => $"Unexpected character: {args[0]}",
            _ when args is [string message] => message,
            
            CompilerExceptionType.UnterminatedString => "Unterminated string",
            CompilerExceptionType.UnrecognizedKeyword => "Unrecognized keyword",
            CompilerExceptionType.UnexpectedEOF => "Unexpected end of file.",
            CompilerExceptionType.InvalidIdentifier => "Invalid identifier.",
            CompilerExceptionType.InvalidNumberLiteral => "Malformed number.",
            CompilerExceptionType.UnclosedParenthesis => "Unclosed parenthesis.",
            CompilerExceptionType.EmptyExpression => "Parenthesis cannot be empty.",
            CompilerExceptionType.MissingValueAfterComma => "Missing value after comma.",
            CompilerExceptionType.MissingExpression => "Missing expression.",

            _ => throw new Exception("Exception type with given arguments (or no arguments) does not have a message."),
        }, args);
}
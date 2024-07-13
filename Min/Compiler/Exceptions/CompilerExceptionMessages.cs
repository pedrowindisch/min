namespace Min.Compiler.Exceptions;

internal static class CompilerExceptionMessages
{
    public static string UnexpectedCharacter(char ch) => $"Unexpected character: {ch}";
    public static string UnterminatedString() => "Unterminated string.";
    public static string UnrecognizedKeyword() => "Unrecognized keyword";
    public static string UnrecognizedKeyword(string closestKeyword) => $"Unrecognized keyword, maybe you meant {closestKeyword}?";
}
namespace Min.Compiler.Exceptions;

internal class CompilerException : Exception
{
    public int Line { get; init; }
    public int Column { get; init; }
    public CompilerExceptionType Type { get; init; }

    public CompilerException(int line, int column, CompilerExceptionType type, params object[] arguments) : base(type.GenerateMessage(arguments))
    {
        Line = line;
        Column = column;
        Type = type;
    }
}
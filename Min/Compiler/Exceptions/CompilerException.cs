namespace Min.Compiler.Exceptions;

public class CompilerException : Exception
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

    public CompilerException(Position position, CompilerExceptionType type, params object[] arguments) : this(position.Line, position.Column, type, arguments) { }
}
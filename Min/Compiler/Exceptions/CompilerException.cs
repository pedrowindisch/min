namespace Min.Compiler.Exceptions;

internal class CompilerException : Exception
{
    private int _line;
    private int _column;

    public CompilerException(int line, int column, string message) : base(message)
    {
        _line = line;
        _column = column;
    }
}
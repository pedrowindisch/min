using Min.Compiler.Nodes;

namespace Min.Compiler.Exceptions;

internal class InternalCompilerException : Exception
{
    public CompilerExceptionType Type { get; init; }

    public InternalCompilerException(CompilerExceptionType type)
    {
        Type = type;
    }

    public InternalCompilerException(CompilerExceptionType type, string message) : base(message)
    {
        Type = type;
    }
}
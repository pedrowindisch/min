using Min.Compiler.Exceptions;

namespace Min.Compiler;

public class SymbolTable
{
    private readonly Dictionary<string, BuiltInType> _symbols = [];

    public bool IsSaved(string symbol) => _symbols.ContainsKey(symbol);

    public void Add(string symbol, BuiltInType type)
    {
        if (IsSaved(symbol))
            throw new InternalCompilerException(CompilerExceptionType.IdentifierAlreadyDeclared);

        _symbols.Add(symbol, type);
    }

    public BuiltInType GetType(string symbol)
    {
        if (!IsSaved(symbol))
            throw new InternalCompilerException(CompilerExceptionType.IdentifierNotDeclared);

        return _symbols[symbol];
    }
}
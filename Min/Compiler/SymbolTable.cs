using Min.Compiler.Exceptions;

namespace Min.Compiler;

public class SymbolTable
{
    private readonly Dictionary<string, TokenType> _symbols = [];

    public bool IsSaved(string symbol) => _symbols.ContainsKey(symbol);

    public void Add(string symbol, TokenType type)
    {
        if (type is not (TokenType.Int or TokenType.Float or TokenType.String or TokenType.Bool))
            throw new Exception("should not be called... invalid type");

        if (IsSaved(symbol))
            throw new InternalCompilerException(CompilerExceptionType.IdentifierAlreadyDeclared);

        _symbols.Add(symbol, type);
    }

    public TokenType GetType(string symbol)
    {
        if (!IsSaved(symbol))
            throw new InternalCompilerException(CompilerExceptionType.IdentifierNotDeclared);

        return _symbols[symbol];
    }
}
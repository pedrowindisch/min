using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

internal class TypeChecker(SymbolTable symbols)
{
    private readonly SymbolTable _symbols = symbols;

    public bool Check(Node node, TokenType expected)
    {
        throw new NotImplementedException();
    }
}
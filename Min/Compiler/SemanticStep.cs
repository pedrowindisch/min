using Min.Compiler.Nodes;

namespace Min.Compiler;

public abstract class SemanticStep<T>(SymbolTable symbols, List<Node> nodes)
{
    protected readonly SymbolTable _symbols = symbols;
    protected readonly List<Node> _nodes = nodes;

    public abstract T Execute(); 
}
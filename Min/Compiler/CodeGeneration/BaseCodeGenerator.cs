using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public abstract class BaseCodeGenerator(SymbolTable symbols, List<Node> nodes)
{
    protected readonly SymbolTable _symbols = symbols;
    protected readonly List<Node> _nodes = nodes;

    public abstract string Execute();
}
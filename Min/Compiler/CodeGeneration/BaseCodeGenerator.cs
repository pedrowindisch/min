using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public abstract class BaseCodeGenerator(SymbolTable symbols, ProgramNode root)
{
    protected readonly SymbolTable _symbols = symbols;
    protected readonly ProgramNode _root = root;

    public abstract string Execute();
}
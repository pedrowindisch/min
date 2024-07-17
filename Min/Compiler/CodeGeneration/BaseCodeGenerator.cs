using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public abstract class BaseCodeGenerator(SymbolTable symbols, List<Node> nodes)
    : SemanticStep<string>(symbols, nodes)
{
    public override abstract string Execute();
}
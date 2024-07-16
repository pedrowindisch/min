using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public abstract class CodeGenerator(List<Node> nodes)
{
    protected List<Node> _nodes = nodes;

    public abstract string Generate();
}
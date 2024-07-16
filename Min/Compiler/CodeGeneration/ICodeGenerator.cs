using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public interface ICodeGenerator
{
    public string Generate(List<Node> nodes);
}
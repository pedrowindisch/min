using System.Collections.Immutable;
using Min.Compiler.Nodes;

namespace Min.Compiler;

public class ConstantFoldingStep : ISemanticAnalysisStep
{
    public (SymbolTable, List<Node>) Execute(SymbolTable symbolTable, List<Node> nodes)
    {
        throw new NotImplementedException();
    }
}
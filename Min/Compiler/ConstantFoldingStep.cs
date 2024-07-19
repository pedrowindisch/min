using System.Collections.Immutable;
using Min.Compiler.Nodes;

namespace Min.Compiler;

public class ConstantFoldingStep : ISemanticAnalysisStep
{
    public (SymbolTable, ProgramNode) Execute(SymbolTable symbolTable, ProgramNode nodes)
    {
        throw new NotImplementedException();
    }
}
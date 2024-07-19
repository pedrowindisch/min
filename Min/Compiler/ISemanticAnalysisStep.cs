using Min.Compiler.Nodes;

namespace Min.Compiler;

internal interface ISemanticAnalysisStep
{
    (SymbolTable, ProgramNode) Execute(SymbolTable symbolTable, ProgramNode nodes);
}
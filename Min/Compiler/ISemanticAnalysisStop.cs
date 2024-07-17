using Min.Compiler.Nodes;

namespace Min.Compiler;

internal interface ISemanticAnalysisStep
{
    (SymbolTable, List<Node>) Execute(SymbolTable symbolTable, List<Node> nodes);
}
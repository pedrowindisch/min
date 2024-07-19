using Min.Compiler.Nodes;

namespace Min.Compiler;

internal class SemanticAnalysisPipeline(SymbolTable symbols, ProgramNode root)
{
    private readonly List<ISemanticAnalysisStep> _steps = [];
    private SymbolTable _symbols = symbols;
    private ProgramNode _root = root;

    public void AddStep<TStep>() where TStep : ISemanticAnalysisStep, new()
    {
        _steps.Add(new TStep());
    }

    public (SymbolTable Symbols, ProgramNode Nodes) Execute()
    {
        foreach (var step in _steps)
            (_symbols, _root) = step.Execute(_symbols, _root);

        return (_symbols, _root);
    }
}
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal class SemanticAnalysisPipeline(SymbolTable symbols, List<Node> nodes)
{
    private readonly List<ISemanticAnalysisStep> _steps = [];
    private SymbolTable _symbols = symbols;
    private List<Node> _nodes = nodes;

    public void AddStep<TStep>() where TStep : ISemanticAnalysisStep, new()
    {
        _steps.Add(new TStep());
    }

    public (SymbolTable Symbols, List<Node> Nodes) Execute()
    {
        foreach (var step in _steps)
            (_symbols, _nodes) = step.Execute(_symbols, _nodes);

        return (_symbols, _nodes);
    }
}
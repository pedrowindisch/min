using Min.Compiler;
using Min.Compiler.CodeGeneration;

namespace Min;

public class Min(string sourceCode) 
{
    private string _sourceCode = sourceCode;

    public string Compile()
    {
        var tokenizer = new Tokenizer(_sourceCode).ToList();
        var nodes = new Parser(tokenizer).Program();
        var symbolTable = new SymbolTable();

        var semanticPipeline = new SemanticAnalysisPipeline(symbolTable, nodes);
        semanticPipeline.AddStep<NameAnalysis>();
        semanticPipeline.AddStep<TypeChecker>();
        // semanticPipeline.AddStep<ConstantFoldingStep>();

        var (Symbols, Nodes) = semanticPipeline.Execute();

        // Study how to allow the user to extend the generator classes.
        BaseCodeGenerator generator = new RawCilGenerator(
            Symbols,
            Nodes
        );

        return generator.Execute();
    }
}
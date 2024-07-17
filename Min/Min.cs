using Min.Compiler;
using Min.Compiler.CodeGeneration;

namespace Min;

public class Min(string sourceCode) 
{
    private string _sourceCode = sourceCode;

    private void ValidateOptions(CompilerOptions options)
    {
        if (File.Exists(options.OutputFilePath))
            throw new Exception("Output file already exists.");
    }

    public void Compile(CompilerOptions options)
    {
        ValidateOptions(options);

        var tokenizer = new Tokenizer(_sourceCode).ToList();
        var nodes = new Parser(tokenizer).Program();
        var symbolTable = new SymbolTable();

        var semanticPipeline = new SemanticAnalysisPipeline(symbolTable, nodes);
        semanticPipeline.AddStep<NameAnalysis>();
        semanticPipeline.AddStep<TypeChecker>();

        var (Symbols, Nodes) = semanticPipeline.Execute();

        // Study how to allow the user to extend the generator classes.
        BaseCodeGenerator generator = new CilGenerator(
            Symbols,
            Nodes
        );
        var output = generator.Execute();

        File.WriteAllText(options.OutputFilePath, output);
    }
}
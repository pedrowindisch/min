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
        var tree = new Parser(tokenizer).Program();
        var symbolTable = new SymbolTable();

        new NameAnalysis(symbolTable, tree).Execute();

        // Study how to allow the user to extend the generator classes.
        BaseCodeGenerator generator = new CilGenerator(symbolTable, tree);
        var output = generator.Execute();

        File.WriteAllText(options.OutputFilePath, output);
    }
}
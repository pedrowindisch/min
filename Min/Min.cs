using System.Text;
using Min.Compiler;
using Min.Compiler.CodeGeneration;
using Min.Compiler.CodeGeneration.Cil;

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

        var output = options
            .Generator
            .Generate(tree);

        File.WriteAllText(options.OutputFilePath, output);
    }
}
using Min.Compiler;
using Min.Compiler.CodeGeneration;
using Min.Compiler.CodeGeneration.Cil;

namespace Min;

public class MinLang
{
    public static void Main(string[] args)
    {
        var tokens = new Tokenizer("output 12.3, 1");
        var tree = new Parser(tokens).Program();

        ICodeGenerator generator = new CilGenerator(tree);
        var code = generator.Generate();

        Console.WriteLine(code);
    }
}
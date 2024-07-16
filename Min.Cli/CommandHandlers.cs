using Min.Compiler.CodeGeneration.Cil;
using Min.Compiler.Exceptions;

namespace Min.Cli;

internal class CommandHandlers
{
    public static void CompileCommandHandler(FileInfo fileInfo)
    {
        if (!File.Exists(fileInfo.FullName))
            throw new ArgumentException("The provided file does not exist.");

        var sourceCode = File.ReadAllText(fileInfo.FullName);
        var compiler = new Min(sourceCode);
        try
        {
            compiler.Compile(new(
                Path.ChangeExtension(fileInfo.FullName, ".comp"),
                new CilGenerator()
            ));
        }
        catch (CompilerException ex)
        {
            var formatter = new CompilerExceptionFormatter(sourceCode);
            var report = formatter.GenerateErrorReport(ex);

            Console.WriteLine(report);
        }
    }
}
using Min.Compiler.Exceptions;
using Spectre.Console;

namespace Min.Cli;

internal class CommandHandlers
{
    public static void CompileCommandHandler(FileInfo fileInfo, bool overwrite)
    {
        if (!File.Exists(fileInfo.FullName))
            throw new ArgumentException("The provided file does not exist.");

        var sourceCode = File.ReadAllText(fileInfo.FullName);
        var compiler = new Min(sourceCode);
        try
        {
            var result = compiler.Compile();

            var outputFile = Path.ChangeExtension(fileInfo.FullName, "comp");
            File.WriteAllText(outputFile, result);
        }
        catch (CompilerException ex)
        {
            var formatter = new CompilerExceptionFormatter(sourceCode);
            var report = formatter.GenerateErrorReport(ex);

            AnsiConsole.Write(report);
        }
    }
}
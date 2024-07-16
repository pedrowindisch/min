using System.Text;
using Min.Compiler.Exceptions;

namespace Min.Cli;

internal class CompilerExceptionFormatter
{
    private string[] Lines { get; init; }

    public CompilerExceptionFormatter(string sourceCode)
    {
        Lines = sourceCode.Split("\n");
    }

    private (int, string) GetLine(int line) => (line, Lines.ElementAt(line - 1)); // Lines are not zero-indexed during the tokenization.
    public string GenerateErrorReport(CompilerException exception)
    {
        List<(int Line, string Content)> linesToPrint = [];

        if (exception.Line > 1)
            linesToPrint.Add(GetLine(exception.Line - 1));

        linesToPrint.Add(GetLine(exception.Line));

        if (exception.Line < Lines.Length)
            linesToPrint.Add(GetLine(exception.Line + 1));

        var basePadding = linesToPrint.Select(l => l.Line.ToString().Length).Max() + 1;
        var report = new StringBuilder(); 

        foreach (var (lineNumber, lineContent) in linesToPrint)
        {
            report.Append($"{lineNumber.ToString().PadLeft(basePadding)} | ");
            report.AppendLine(lineContent);

            if (lineNumber == exception.Line)
            {
                report.Append($"{"".PadLeft(basePadding)} |");
                report.Append(new string(' ', exception.Column + 1));
                report.AppendLine("^");
            }
        }

        report.AppendLine();
        report.AppendLine(exception.Message);

        return report.ToString();
    }
}
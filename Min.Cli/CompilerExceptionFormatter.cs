using System.Linq;
using System.Text;
using Min.Compiler.Exceptions;
using Spectre.Console;

namespace Min.Cli;

internal class CompilerExceptionFormatter
{
    private string[] Lines { get; init; }

    public CompilerExceptionFormatter(string sourceCode)
    {
        Lines = sourceCode.Split("\n");
    }

    private (int, string) GetLine(int line) => (line, Lines.ElementAt(line - 1)); // Lines are not zero-indexed during the tokenization.
    public Markup GenerateErrorReport(CompilerException exception)
    {
        List<(int Line, string Content)> linesToPrint = [];

        if (exception.Line > 1)
            linesToPrint.Add(GetLine(exception.Line - 1));

        linesToPrint.Add(GetLine(exception.Line));

        if (exception.Line < Lines.Length)
            linesToPrint.Add(GetLine(exception.Line + 1));

        var basePadding = linesToPrint.Select(l => l.Line.ToString().Length).Max() + 1;
        var report = new List<string>(); 

        foreach (var (lineNumber, lineContent) in linesToPrint)
        {
            var markup = $"{lineNumber.ToString().PadLeft(basePadding)} | ";
            markup += lineContent;

            if (lineNumber == exception.Line)
            {
                markup = "[bold red]" + markup + '\n';
                
                markup += $"{"".PadLeft(basePadding)} |";
                markup += new string(' ', exception.Column + 1);
                markup += $"^ [red]{exception.Message}[/] [/]";
            }

            report.Add(markup);
        }

        return new Markup(string.Join('\n', report));
    }
}
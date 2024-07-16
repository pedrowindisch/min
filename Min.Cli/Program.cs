using System.CommandLine;
using Min.Cli;

var fileArgument = new Argument<FileInfo>(
    name: "file path",
    description: "source code file"
);

var rootCommand = new RootCommand("Min compiler - CLI");
rootCommand.AddArgument(fileArgument);

rootCommand.SetHandler(CommandHandlers.CompileCommandHandler, fileArgument);

await rootCommand.InvokeAsync(args);
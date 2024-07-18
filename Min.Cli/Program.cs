using System.CommandLine;
using Min.Cli;

var fileArgument = new Argument<FileInfo>(
    name: "file path",
    description: "source code file"
);

var overwrite = new Option<bool>(
    name: "--overwrite",
    description: "Whether to overwrite the output file if it already exists or not.",
    getDefaultValue: () => false
);
overwrite.AddAlias("-o");

var rootCommand = new RootCommand("Min compiler - CLI");
rootCommand.AddArgument(fileArgument);

rootCommand.AddOption(overwrite);

rootCommand.SetHandler(CommandHandlers.CompileCommandHandler, fileArgument, overwrite);

await rootCommand.InvokeAsync(args);
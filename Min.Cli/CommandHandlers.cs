namespace Min.Cli;

internal class CommandHandlers
{
    public static void CompileCommandHandler(FileInfo fileInfo)
    {
        if (!File.Exists(fileInfo.FullName))
            throw new ArgumentException("The provided file does not exist.");

        var compiler = new Min(File.ReadAllText(fileInfo.FullName));
        compiler.Compile(new(
            Path.ChangeExtension(fileInfo.FullName, ".comp")
        ));
    }
}
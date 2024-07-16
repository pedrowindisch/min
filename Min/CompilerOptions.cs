using Min.Compiler.CodeGeneration;

namespace Min;

public record CompilerOptions(
    string OutputFilePath,
    ICodeGenerator Generator
);
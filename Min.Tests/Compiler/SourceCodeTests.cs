using Min.Compiler.Exceptions;
using Min.Tests.Utils;

namespace Min.Tests.Compiler;

public class SourceCodeTests
{
    /*
        This test class uses the samples provided in the Min.Tests/Samples folders.

        We don't check whether the compiler generated the correct output/object code, as the test classes 
        for each target/code generation method are responsible for this.
    */

    [Theory(DisplayName = "Valid source code tests")]
    [SamplesData("ValidSamples")]
    public void Compile_ValidSource_ReturnsCilCode(params string[] fileLines)
    {
        var compiler = new Min(string.Join('\n', fileLines));
        compiler.Compile();
    }

    [Theory(DisplayName = "Invalid source code should throw an exception")]
    [SamplesData("InvalidSamples")]
    public void Compile_InvalidSourceCode_ThrowsException(params string[] fileLines)
    {
        var compiler = new Min(string.Join('\n', fileLines));

        Assert.Throws<CompilerException>(compiler.Compile);
    }
}
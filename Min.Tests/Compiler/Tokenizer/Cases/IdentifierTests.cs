using Min.Compiler;
using Min.Compiler.Exceptions;

namespace Min.Tests.Compiler.TokenizerTests.Cases;

[Trait("Tokenizer", "Identifier tests")]
public class IdentifierTests
{
    [Theory]
    [InlineData(".name")]
    [InlineData(".year2")]
    [InlineData(".2year")]
    [InlineData(".123")]
    [InlineData(".y")]
    public void ValidIdentifier(string identifier)
    {
        var sourceCode = $"{identifier} int";
        var tokenizer = new Tokenizer(sourceCode);

        Assert.Equivalent(new List<Token>
        {
            new(1, 0, TokenType.Identifier, identifier.TrimStart('.')),
            new(1, identifier.Length + 1, TokenType.Int),
            new(1, sourceCode.Length, TokenType.EOF)
        }, tokenizer.ToList());
    }


    [Theory]
    // [InlineData(".1name")]
    [InlineData(".__name")]
    [InlineData(".\"name\"")]
    [InlineData(".name_with_underscores")]
    public void InvalidIdentifier_ThrowsException(string invalidIdentifier)
    {
        var tokenizer = new Tokenizer($"{invalidIdentifier} int = 123");

        var exception = Assert.Throws<CompilerException>(tokenizer.ToList);
        Assert.Equal(CompilerExceptionType.InvalidIdentifier, exception.Type);
    }
}
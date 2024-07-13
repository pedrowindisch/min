using Min.Compiler;
using Min.Compiler.Exceptions;

namespace Min.Tests.Compiler.Tokenizer.Cases;

public class StringLiteralTests
{
    [Theory]
    [InlineData(@"""""")]
    [InlineData(@"""testing""")]
    [InlineData(@"""123""")]
    [InlineData(@""".123""")]
    public void Tokenize_StringLiterals_ReturnsTokensList(string stringLiteral)
    {
        var tokenizer = new Min.Compiler.Tokenizer(stringLiteral);

        Assert.Equivalent(new List<Token>
        {
            new Token(1, 0, TokenType.StringLiteral, stringLiteral.Trim('"')),
            new Token(1, stringLiteral.Length, TokenType.EOF)
        }, tokenizer.ToList());
    }

    [Theory]
    [InlineData(@"""")]
    [InlineData(@"""testing")]
    public void Tokenize_InvalidStringLiterals_ThrowsException(string stringLiteral)
    {
        var tokenizer = new Min.Compiler.Tokenizer(stringLiteral);

        var exception = Assert.Throws<CompilerException>(tokenizer.ToList);
        Assert.Equal(CompilerExceptionType.UnterminatedString, exception.Type);
    }
}
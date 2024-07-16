using Min.Compiler;
using Min.Compiler.Exceptions;

namespace Min.Tests.Compiler.TokenizerTests.Cases;

public class NumberLiteralTests
{
    [Theory]
    [InlineData("123")]
    [InlineData("123.3")]
    public void Tokenize_NumberLiterals_ReturnsTokensList(string numberLiteral)
    {
        var tokenizer = new Tokenizer(numberLiteral);

        Assert.Equivalent(new List<Token>
        {
            new Token(1, 0, TokenType.NumberLiteral, numberLiteral),
            new Token(1, numberLiteral.Length, TokenType.EOF)
        }, tokenizer.ToList());
    } 

    [Theory]
    [InlineData("123.")]
    public void Tokenize_InvalidNumberLiterals_ThrowsException(string numberLiteral)
    {
        var tokenizer = new Tokenizer(numberLiteral);

        var exception = Assert.Throws<CompilerException>(tokenizer.ToList);
        Assert.Equal(CompilerExceptionType.InvalidNumberLiteral, exception.Type);
    } 
}
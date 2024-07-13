using Min.Compiler;

namespace Min.Tests.Compiler.Tokenizer.Cases;

public class NumberLiteralTests
{
    [Theory]
    [InlineData("123")]
    [InlineData("123.3")]
    public void Tokenize_NumberLiterals_ReturnsTokensList(string numberLiteral)
    {
        var tokenizer = new Min.Compiler.Tokenizer(numberLiteral);

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
        var tokenizer = new Min.Compiler.Tokenizer(numberLiteral);

        Assert.ThrowsAny<Exception>(tokenizer.ToList);
    } 
}
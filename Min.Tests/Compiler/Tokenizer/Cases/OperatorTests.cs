using Min.Compiler;
using Min.Compiler.Exceptions;

namespace Min.Tests.Compiler.Tokenizer.Cases;

public class OperatorTests
{
    [Theory]
    [InlineData("*", TokenType.Multiply)]
    [InlineData("/", TokenType.Divide)]
    [InlineData("+", TokenType.Add)]
    [InlineData("-", TokenType.Subtract)]
    [InlineData("==", TokenType.EqualsTo)]
    [InlineData("=", TokenType.Assign)]
    [InlineData("!=", TokenType.NotEqualsTo)]
    [InlineData(">=", TokenType.GreaterThanOrEqual)]
    [InlineData("<=", TokenType.LessThanOrEqual)]
    [InlineData(">", TokenType.GreaterThan)]
    [InlineData("<", TokenType.LessThan)]
    internal void Tokenize_Operator_ReturnsTokensList(string op, TokenType expectedType)
    {
        var tokenizer = new Min.Compiler.Tokenizer(op);

        Assert.Equivalent(new List<Token>()
        {
            new Token(1, 0, expectedType),
            new Token(1, op.Length, TokenType.EOF),
        }, tokenizer.ToList());
    }

    [Theory]
    [InlineData("**")]
    [InlineData("--")]
    [InlineData(">>")]
    [InlineData("=>")]
    [InlineData("!*")]
    [InlineData("//")]
    public void Tokenize_Operator_ThrowsException(string invalidOperator)
    {
        var tokenizer = new Min.Compiler.Tokenizer(invalidOperator);

        var exception = Assert.Throws<CompilerException>(tokenizer.ToList);
        Assert.Equal(
            CompilerExceptionType.UnrecognizedOperator, 
            exception.Type
        );
    }
}
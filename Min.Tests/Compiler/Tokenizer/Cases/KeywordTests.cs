using Min.Compiler;
using Min.Compiler.Exceptions;

namespace Min.Tests.Compiler.Tokenizer.Cases;

public class KeywordTests
{
    [Fact]
    public void Tokenize_InputOutputKeywords_ReturnsTokensList()
    {
        var sourceCode = "input .nome\noutput .nome";
        var tokenizer = new Min.Compiler.Tokenizer(sourceCode);

        Assert.Equivalent(new List<Token>()
        {
            new Token(1, 0, TokenType.Input),
            new Token(1, 6, TokenType.Identifier, "nome"),
            new Token(2, 0, TokenType.Output),
            new Token(2, 7, TokenType.Identifier, "nome"),
        }, tokenizer.ToList());
    }

    [Fact]
    public void Tokenize_InOutKeywords_ReturnsTokensList()
    {
        var sourceCode = "in .nome\nout .nome";
        var tokenizer = new Min.Compiler.Tokenizer(sourceCode);

        Assert.Equivalent(new List<Token>()
        {
            new Token(1, 0, TokenType.Input),
            new Token(1, 3, TokenType.Identifier, "nome"),
            new Token(2, 0, TokenType.Output),
            new Token(2, 4, TokenType.Identifier, "nome"),
        }, tokenizer.ToList());
    }

    [Fact]
    public void Tokenize_Types_ReturnsTokensList()
    {
        var sourceCode = "int string bool";
        var tokenizer = new Min.Compiler.Tokenizer(sourceCode);

        Assert.Equivalent(new List<Token>()
        {
            new Token(1, 0, TokenType.Int),
            new Token(1, 4, TokenType.String),
            new Token(1, 11, TokenType.Bool),
            new Token(1, 15, TokenType.EOF),
        }, tokenizer.ToList());
    }

    [Fact]
    public void Tokenize_UnrecognizedKeyword_ThrowsException()
    {
        var sourceCode = ".name nonexistentkeyword";
        var tokenizer = new Min.Compiler.Tokenizer(sourceCode);

        var exception = Assert.Throws<CompilerException>(tokenizer.ToList);
        Assert.Equal(
            CompilerExceptionType.UnrecognizedKeyword,
            exception.Type
        );
    }
}
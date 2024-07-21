using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class InputStatementTests
{
    [Fact]
    public void Parse_InputStatementWithIdentifierToken_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Input),
            new Token(1, 0, TokenType.Identifier, ".min"),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        Assert.Equivalent(new ProgramNode([
            new InputStatementNode(
                Position.From(tokens[0]),
                ".min"
            )
        ]), parser.Program());
    } 

    [Fact]
    public void Parse_InputStatementWithNonIdentifierToken_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Input),
            new Token(1, 0, TokenType.StringLiteral, "min"),
            new Token(1, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.ExpectedKeyword, exception.Type);
    }
}
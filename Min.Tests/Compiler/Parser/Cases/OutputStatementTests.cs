using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class OutputStatementTests
{
    [Fact]
    public void Parse_OutputWithSingleArgument_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Output),
            new Token(1, 0, TokenType.True),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        Assert.Equivalent(new ProgramNode([
            new OutputStatementNode(
                Position.From(tokens[0]),
                [
                    new BooleanExpressionNode(Position.From(tokens[1]), true)
                ]
            )
        ]), parser.Program());
    } 

    [Fact]
    public void Parse_OutputWithMultipleArguments_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Output),
            new Token(1, 0, TokenType.True),
            new Token(1, 0, TokenType.Comma),
            new Token(1, 0, TokenType.StringLiteral, "min"),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        Assert.Equivalent(new ProgramNode([
            new OutputStatementNode(
                Position.From(tokens[0]),
                [
                    new BooleanExpressionNode(Position.From(tokens[1]), true),
                    new StringExpressionNode(Position.From(tokens[3]), "min")
                ]
            )
        ]), parser.Program());
    }

    [Fact]
    public void Parse_OutputWithNoArguments_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Output),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.MissingExpression, exception.Type);
    }

    [Fact]
    public void Parse_MalformedCommaSeparatedArguments_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.Output),
            new Token(1, 0, TokenType.StringLiteral, "min"),
            new Token(1, 0, TokenType.Comma),
            new Token(1, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.MissingValueAfterComma, exception.Type);
    }
}
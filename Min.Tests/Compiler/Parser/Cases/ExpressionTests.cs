using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class ExpressionTests
{
    [Theory]
    [InlineData(TokenType.NumberLiteral, 123, 321)]
    [InlineData(TokenType.StringLiteral, "name", "year")]
    [InlineData(TokenType.NumberLiteral, 1.23, 32.1)]
    internal void Parse_BinaryExpressionLiterals_ReturnsTree(TokenType literalType, object firstLiteral, object secondLiteral)
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Output),
            new(1, 0, literalType, firstLiteral.ToString()!),
            new(1, 1, TokenType.Add),
            new(1, 2, literalType, secondLiteral.ToString()!),
            new(1, 3, TokenType.EOF),
        };

        var parser = new Parser(tokens);
        var output = parser.Program()[0] as OutputStatementNode;

        Assert.Equivalent(new BinaryExpressionNode(
            new LiteralNode(tokens[1]),
            TokenType.Add,
            new LiteralNode(tokens[3])
        ), output!.Values[0]);
    }

    [Fact]
    internal void Parse_BinaryExpressionIdentifiers_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Output),
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 1, TokenType.Add),
            new(1, 2, TokenType.Identifier, ".year"),
            new(1, 3, TokenType.EOF),
        };

        var parser = new Parser(tokens);
        var output = parser.Program()[0] as OutputStatementNode;

        Assert.Equivalent(new BinaryExpressionNode(
            new VariableNode(tokens[1]),
            TokenType.Add,
            new VariableNode(tokens[3])
        ), output!.Values[0]);
    }

    [Fact]
    public void Parse_ExpressionWithGroupings_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Output),
            new(1, 0, TokenType.NumberLiteral, "1"),
            new(1, 0, TokenType.Add),
            new(1, 0, TokenType.LeftParenthesis),
            new(1, 0, TokenType.NumberLiteral, "2"),
            new(1, 0, TokenType.Add),
            new(1, 0, TokenType.NumberLiteral, "3"),
            new(1, 0, TokenType.RightParenthesis),
            new(1, 0, TokenType.EOF),
        };

        var parser = new Parser(tokens);
        var output = parser.Program()[0] as OutputStatementNode;

        Assert.Equivalent(new BinaryExpressionNode(
            new LiteralNode(tokens[1]),
            TokenType.Add,
            new GroupingNode(new BinaryExpressionNode(
                new LiteralNode(tokens[4]),
                TokenType.Add,
                new LiteralNode(tokens[6])
            ))
        ), output!.Values[0]);
    }

    [Fact]
    public void Parse_GroupingWithoutExpression_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Output),
            new(1, 0, TokenType.NumberLiteral, "1"),
            new(1, 0, TokenType.Add),
            new(1, 0, TokenType.LeftParenthesis),
            new(1, 0, TokenType.RightParenthesis),
            new(1, 0, TokenType.EOF),
        };

        var parser = new Parser(tokens);
        var exception = Assert.Throws<CompilerException>(parser.Program);        
    }

    [Fact]
    public void Parse_GroupingWithoutClosingParenthesis_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Output),
            new(1, 0, TokenType.NumberLiteral, "1"),
            new(1, 0, TokenType.Add),
            new(1, 0, TokenType.LeftParenthesis),
            new(1, 0, TokenType.NumberLiteral, "2"),
            new(1, 0, TokenType.Add),
            new(1, 0, TokenType.NumberLiteral, "3"),
            new(2, 0, TokenType.Identifier, ".nome"),
            new(2, 0, TokenType.Int),
            new(1, 0, TokenType.EOF),
        };

        var parser = new Parser(tokens);
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.UnclosedParenthesis, exception.Type);        
    }
}
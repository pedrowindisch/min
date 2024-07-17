using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class IfStatementTests
{
    [Fact]
    public void Parse_SingleBranchIfStatement_ReturnsTree()
    {
        // if true:
        //     output "min"
        // endif
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.If),
            new Token(1, 0, TokenType.True),
            new Token(1, 0, TokenType.Colon),
            new Token(2, 0, TokenType.Output),
            new Token(2, 0, TokenType.StringLiteral, "min"),
            new Token(3, 0, TokenType.EndIf),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        Assert.Equivalent(new List<Node>()
        {
            new IfStatementNode(
                tokens[0], 
                new LiteralNode(tokens[1]),
                [
                    new OutputStatementNode(
                        tokens[3], 
                        [new LiteralNode(tokens[4])]
                    )
                ]
            )
        }, parser.Program());
    }

    
    [Fact]
    public void Parse_SingleBranchIfStatementWithExpressionCondition_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.If),
            new Token(1, 0, TokenType.NumberLiteral, "1"),
            new Token(1, 0, TokenType.Add),
            new Token(1, 0, TokenType.NumberLiteral, "2"),
            new Token(1, 0, TokenType.EqualsTo),
            new Token(1, 0, TokenType.NumberLiteral, "3"),
            new Token(1, 0, TokenType.Colon),
            new Token(2, 0, TokenType.Output),
            new Token(2, 0, TokenType.StringLiteral, "min"),
            new Token(3, 0, TokenType.EndIf),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        Assert.Equivalent(new List<Node>()
        {
            new IfStatementNode(
                tokens[0], 
                [new BinaryExpressionNode(
                    new BinaryExpressionNode(
                        new LiteralNode(tokens[1]),
                        TokenType.Add,
                        new LiteralNode(tokens[3])
                    ),
                    TokenType.EqualsTo,
                    new LiteralNode(tokens[5])
                )]
            )
        }, parser.Program());
    }

    [Fact]
    public void Parse_UnfinishedSingleBranchIfStatement_ThrowsException()
    {
        
        // if true:
        //     output "min"
        // -- end of file
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.If),
            new Token(1, 0, TokenType.True),
            new Token(1, 0, TokenType.Colon),
            new Token(2, 0, TokenType.Output),
            new Token(2, 0, TokenType.StringLiteral, "min"),
            new Token(3, 0, TokenType.EOF)
        };

        var parser = new Parser(tokens);
        
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.UnexpectedEOF, exception.Type);
    }
}
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
        Assert.Equivalent(new ProgramNode([
            new IfStatementNode(
                Position.From(tokens[0]),
                new BooleanExpressionNode(Position.From(tokens[1]), true),
                [
                    new OutputStatementNode(
                        Position.From(tokens[3]),
                        [
                            new StringExpressionNode(
                                Position.From(tokens[4]),
                                "min"
                            )
                        ]
                    )
                ]
            )
        ]), parser.Program());
    }

    
    [Fact]
    public void Parse_SingleBranchIfStatementWithExpressionCondition_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.If),
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
        Assert.Equivalent(new ProgramNode([
            new IfStatementNode(
                Position.From(tokens[0]),
                new ComparisonExpressionNode(
                    Position.From(tokens[1]),
                    new NumberExpressionNode(Position.From(tokens[1]), 2),
                    BuiltInOperator.EqualsTo,
                    new NumberExpressionNode(Position.From(tokens[3]), 3)
                ),
                [
                    new OutputStatementNode(
                        Position.From(tokens[5]),
                        [
                            new StringExpressionNode(
                                Position.From(tokens[6]),
                                "min"
                            )
                        ]
                    )
                ]
            )
        ]), parser.Program());
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
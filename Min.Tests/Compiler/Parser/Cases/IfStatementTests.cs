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
                ],
                [],
                null
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
                ],
                [],
                null
            )
        ]), parser.Program());
    }

    [Fact]
    public void Parse_IfStatementWithSingleElseIfBranch_ReturnsTree()
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
            new Token(3, 0, TokenType.Else),
            new Token(3, 0, TokenType.If),
            new Token(3, 0, TokenType.True, "true"),
            new Token(3, 0, TokenType.Colon),
            new Token(4, 0, TokenType.Output),
            new Token(4, 0, TokenType.StringLiteral, "min"),
            new Token(5, 0, TokenType.EndIf),
            new Token(5, 0, TokenType.EOF)
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
                ],
                [
                    new ElseIfStatementNode(
                        Position.From(tokens[7]),
                        new BooleanExpressionNode(Position.From(tokens[9]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[11]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[12]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    )
                ],
                null
            )
        ]), parser.Program());
    }    

    [Fact]
    public void Parse_IfStatementWithMultipleElseIfBranches_ReturnsTree()
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
            new Token(3, 0, TokenType.Else),
            new Token(3, 0, TokenType.If),
            new Token(3, 0, TokenType.True, "true"),
            new Token(3, 0, TokenType.Colon),
            new Token(4, 0, TokenType.Output),
            new Token(4, 0, TokenType.StringLiteral, "min"),
            new Token(5, 0, TokenType.Else),
            new Token(5, 0, TokenType.If),
            new Token(5, 0, TokenType.True, "true"),
            new Token(5, 0, TokenType.Colon),
            new Token(6, 0, TokenType.Output),
            new Token(6, 0, TokenType.StringLiteral, "min"),
            new Token(7, 0, TokenType.EndIf),
            new Token(7, 0, TokenType.EOF)
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
                ],
                [
                    new ElseIfStatementNode(
                        Position.From(tokens[7]),
                        new BooleanExpressionNode(Position.From(tokens[9]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[11]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[12]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    ),
                    new ElseIfStatementNode(
                        Position.From(tokens[13]),
                        new BooleanExpressionNode(Position.From(tokens[15]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[17]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[18]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    )
                ],
                null
            )
        ]), parser.Program());
    }

    
    [Fact]
    public void Parse_IfStatementWithMultipleElseIfBranchesWithElseBranch_ReturnsTree()
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
            new Token(3, 0, TokenType.Else),
            new Token(3, 0, TokenType.If),
            new Token(3, 0, TokenType.True, "true"),
            new Token(3, 0, TokenType.Colon),
            new Token(4, 0, TokenType.Output),
            new Token(4, 0, TokenType.StringLiteral, "min"),
            new Token(5, 0, TokenType.Else),
            new Token(5, 0, TokenType.If),
            new Token(5, 0, TokenType.True, "true"),
            new Token(5, 0, TokenType.Colon),
            new Token(6, 0, TokenType.Output),
            new Token(6, 0, TokenType.StringLiteral, "min"),
            new Token(7, 0, TokenType.Else),
            new Token(7, 0, TokenType.Colon),
            new Token(8, 0, TokenType.Output),
            new Token(8, 0, TokenType.StringLiteral, "min"),
            new Token(9, 0, TokenType.EndIf),
            new Token(9, 0, TokenType.EOF)
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
                ],
                [
                    new ElseIfStatementNode(
                        Position.From(tokens[7]),
                        new BooleanExpressionNode(Position.From(tokens[9]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[11]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[12]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    ),
                    new ElseIfStatementNode(
                        Position.From(tokens[13]),
                        new BooleanExpressionNode(Position.From(tokens[15]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[17]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[18]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    )
                ],
                new ElseStatementNode(
                    Position.From(tokens[19]),
                    [
                        new OutputStatementNode(
                            Position.From(tokens[21]),
                            [
                                new StringExpressionNode(
                                    Position.From(tokens[22]),
                                    "min"
                                )
                            ]
                        )
                    ]
                )
            )
        ]), parser.Program());
    }

    [Fact]
    public void Parse_NestedIfStatements_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new Token(1, 0, TokenType.If),
            new Token(1, 0, TokenType.NumberLiteral, "2"),
            new Token(1, 0, TokenType.EqualsTo),
            new Token(1, 0, TokenType.NumberLiteral, "3"),
            new Token(1, 0, TokenType.Colon),
            new Token(3, 0, TokenType.If),
            new Token(3, 0, TokenType.True, "true"),
            new Token(3, 0, TokenType.Colon),
            new Token(4, 0, TokenType.Output),
            new Token(4, 0, TokenType.StringLiteral, "min"),
            new Token(8, 0, TokenType.EndIf),
            new Token(9, 0, TokenType.EndIf),
            new Token(9, 0, TokenType.EOF)
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
                    new IfStatementNode(
                        Position.From(tokens[5]),
                        new BooleanExpressionNode(Position.From(tokens[6]), true),
                        [
                            new OutputStatementNode(
                                Position.From(tokens[8]),
                                [
                                    new StringExpressionNode(
                                        Position.From(tokens[9]),
                                        "min"
                                    )
                                ]
                            )
                        ]
                    )
                    
                ],
                null
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

    [Fact]
    public void Parse_UnfinishedNestedIfStatement_ThrowsException()
    {
        var tokens = new List<Token>()
        {
             new Token(1, 0, TokenType.If),
            new Token(1, 0, TokenType.NumberLiteral, "2"),
            new Token(1, 0, TokenType.EqualsTo),
            new Token(1, 0, TokenType.NumberLiteral, "3"),
            new Token(1, 0, TokenType.Colon),
            new Token(3, 0, TokenType.If),
            new Token(3, 0, TokenType.True, "true"),
            new Token(3, 0, TokenType.Colon),
            new Token(4, 0, TokenType.Output),
            new Token(4, 0, TokenType.StringLiteral, "min"),
            new Token(8, 0, TokenType.EndIf),
            new Token(9, 0, TokenType.EOF)
       };

        var parser = new Parser(tokens);
        
        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.UnexpectedEOF, exception.Type);
    }
}
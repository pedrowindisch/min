using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class VariableAssignmentStatementTests
{
    [Fact(DisplayName = "Should return the parse tree when evaluating a correct assignment statement.")]
    public void Parse_AssignmentStatement_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 5, TokenType.Assign),
            new(1, 5, TokenType.StringLiteral, "min"),
            new(1, 10, TokenType.EOF),
        };

        var parser = new Parser(tokens);

        Assert.Equivalent(new ProgramNode([new VariableAssignmentNode(
            Position.From(tokens[0]),
            ".name", 
            new StringExpressionNode(Position.From(tokens[2]), "min")
        )]), parser.Program());
    }

    [Fact(DisplayName = "Should throw an exception when evaluating an invalid assignment statement.")]
    public void Parse_InvalidAssignmentsStatement_ThrowsException()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 5, TokenType.Assign),
            new(1, 5, TokenType.String),
            new(1, 10, TokenType.EOF),
        };

        var parser = new Parser(tokens);

        var exception = Assert.Throws<CompilerException>(parser.Program);
        Assert.Equal(CompilerExceptionType.InvalidAssignmentValue, exception.Type);
    }
}
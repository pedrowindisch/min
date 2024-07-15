using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.Parser.Cases;

public class AssignmentStatementTests
{
    [Fact(DisplayName = "Should return the parse tree when evaluating a correct assignment statement.")]
    public void Parse_AssignmentsStatement_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 5, TokenType.Assign),
            new(1, 5, TokenType.StringLiteral, "min"),
            new(1, 10, TokenType.EOF),
        };

        var parser = new Min.Compiler.Parser(tokens);

        Assert.Equivalent(new List<Node>()
        {
            new AssignmentStatementNode(tokens[0], new LiteralNode(tokens[2]))
        }, parser.Program());
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

        var parser = new Min.Compiler.Parser(tokens);

        var exception = Assert.Throws<CompilerException>(parser.Program);
        // Assert.Equal(CompilerExceptionType.InvalidVariableDeclaration, exception.Type);
    }
}
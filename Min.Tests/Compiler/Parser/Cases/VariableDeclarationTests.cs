using Min.Compiler;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.ParserTests.Cases;

public class VariableDeclarationTests
{
    [Fact]
    public void Parse_VariableDeclarationNoInitialValue_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 5, TokenType.String),
            new(1, 10, TokenType.EOF),
        };

        var parser = new Parser(tokens);

        Assert.Equivalent(new ProgramNode([
            new VariableDeclarationNode(
                Position.From(tokens[0]),
                ".name",
                BuiltInType.String
            )
        ]), parser.Program());
    }

    [Fact]
    public void Parse_VariableDeclarationWithInitialValue_ReturnsTree()
    {
        var tokens = new List<Token>()
        {
            new(1, 0, TokenType.Identifier, ".name"),
            new(1, 5, TokenType.String),
            new(1, 10, TokenType.Assign),
            new(1, 11, TokenType.StringLiteral, "min"),
            new(1, 15, TokenType.EOF),
        };

        var parser = new Parser(tokens);

        Assert.Equivalent(new ProgramNode([
            new VariableDeclarationNode(
                Position.From(tokens[0]),
                ".name",
                BuiltInType.String,
                new StringExpressionNode(
                    Position.From(tokens[3]), "min"
                )
            )
        ]), parser.Program());
    }
}
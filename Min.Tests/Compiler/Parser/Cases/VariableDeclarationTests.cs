using Min.Compiler;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.Parser.Cases;

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

        var parser = new Min.Compiler.Parser(tokens);

        Assert.Equivalent(new List<Node>()
        {
            new VariableDeclarationNode(tokens[0], TokenType.String, ".name")
        }, parser.Program());
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

        var parser = new Min.Compiler.Parser(tokens);

        Assert.Equivalent(new List<Node>()
        {
            new VariableDeclarationNode(tokens[0], TokenType.String, ".name", new LiteralNode(new Token(1, 11, TokenType.StringLiteral, "min")))
        }, parser.Program());
    }
}
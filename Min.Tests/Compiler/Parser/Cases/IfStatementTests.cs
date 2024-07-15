using Min.Compiler;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler.Parser.Cases;

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

        var parser = new Min.Compiler.Parser(tokens);
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
}
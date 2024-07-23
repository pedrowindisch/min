using Min.Compiler;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Tests;

public class NameAnalysisTests
{
    [Fact]
    public void Execute_ShouldAddVariableToSymbolTable_WhenVariableDeclarationNodeIsVisited()
    {
        // Arrange
        var symbols = new SymbolTable();
        var variableDeclarationNode = new VariableDeclarationNode(
            new Position(0, 0), 
            "x", 
            BuiltInType.Int,
            new NumberExpressionNode(new Position(0, 1), 123)
        );

        var program = new ProgramNode([variableDeclarationNode]);

        var nameAnalysis = new NameAnalysis();
        var (symbolTabel, _) = nameAnalysis.Execute(symbols, program);

        Assert.True(symbolTabel.IsSaved("x"));
        Assert.Equal(BuiltInType.Int, symbols.GetType("x"));
    }

    [Fact]
    public void Visit_VariableAssignmentNodeWhenIdentifierIsNotDeclared_ThrowsCompilerException()
    {
        var symbols = new SymbolTable();
        var variableAssignmentNode = new VariableAssignmentNode(new Position(0,0), "x", new NumberExpressionNode(new Position(0, 0), 123));
        var nameAnalysis = new NameAnalysis();

        var error = Assert.Throws<CompilerException>(() => nameAnalysis.Execute(symbols, new ProgramNode([variableAssignmentNode])));
        Assert.Equal(CompilerExceptionType.IdentifierNotDeclared, error.Type);
    }

    [Fact]
    public void Execute_InputStatementNodeWhenIdentifierIsNotDeclared_ThrowsException()
    {
        var symbols = new SymbolTable();
        var inputStatementNode = new InputStatementNode(new Position(0,0), "min");
        var nameAnalysis = new NameAnalysis();

        var error = Assert.Throws<CompilerException>(() => nameAnalysis.Execute(symbols, new ProgramNode([inputStatementNode])));
        Assert.Equal(CompilerExceptionType.IdentifierNotDeclared, error.Type);
    }

    [Fact]
    public void Execute_OutputStatementNodeWhenIdentifierIsNotDeclared_ThrowsException()
    {
        var symbols = new SymbolTable();
        var inputStatementNode = new OutputStatementNode(new Position(0,0), [new IdentifierExpressionNode(new Position(0, 0), "min")]);
        var nameAnalysis = new NameAnalysis();

        var error = Assert.Throws<CompilerException>(() => nameAnalysis.Execute(symbols, new ProgramNode([inputStatementNode])));
        Assert.Equal(CompilerExceptionType.IdentifierNotDeclared, error.Type);
    }
    
    [Fact]
    public void Execute_DeclareVariableTwice_ThrowsException()
    {
        var symbols = new SymbolTable();
        var variableDeclarationNode = new VariableDeclarationNode(
            new Position(0, 0), 
            "x", 
            BuiltInType.Int,
            new NumberExpressionNode(new Position(0, 1), 123)
        );

        var program = new ProgramNode([variableDeclarationNode, variableDeclarationNode]);

        var nameAnalysis = new NameAnalysis();
        var error = Assert.Throws<CompilerException>(() => nameAnalysis.Execute(symbols, program));
        Assert.Equal(CompilerExceptionType.IdentifierAlreadyDeclared, error.Type);
    }
}
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal sealed class NameAnalysis
    : ISemanticAnalysisStep, IVisitor
{
    private SymbolTable _symbols = null!;

    public (SymbolTable, ProgramNode) Execute(SymbolTable symbols, ProgramNode program)
    {
        _symbols = symbols;

        foreach (var node in program.Statements)
            node.Accept(this);

        return (symbols, program);
    }

    public void Visit(VariableDeclarationNode node)
    {
        try
        {
            _symbols.Add(node.Identifier, node.Type);
        }
        catch (InternalCompilerException ex) when (ex.Type is CompilerExceptionType.IdentifierAlreadyDeclared)
        {
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierAlreadyDeclared, node.Identifier);
        }
    }

    public void Visit(ComparisonExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public void Visit(UnaryExpressionNode node)
    {
        node.Value.Accept(this);
    }

    public void Visit(IdentifierExpressionNode node)
    {
        if (!_symbols.IsSaved(node.Identifier))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Identifier);
    }

    public void Visit(GroupingExpressionNode node)
    {
        node.Value.Accept(this);
    }

    public void Visit(VariableAssignmentNode node)
    {
        if (!_symbols.IsSaved(node.Identifier))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Identifier);
    }

    public void Visit(InputStatementNode node)
    {
        if (!_symbols.IsSaved(node.Identifier))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Identifier);
    }

    public void Visit(OutputStatementNode node)
    {
        foreach (var value in node.Values)
            value.Accept(this);
    }

    public void Visit(IfStatementNode node)
    {
        node.Condition.Accept(this);
    }

    public void Visit(MultiplicativeExpressionNode node) 
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public void Visit(AdditiveExpressionNode node) 
    {
        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public void Visit(BooleanExpressionNode node) { }
    public void Visit(NumberExpressionNode node) {}
    public void Visit(ProgramNode node)
    {
        throw new Exception("Should never have two program nodes inside the same program.");
    }
}
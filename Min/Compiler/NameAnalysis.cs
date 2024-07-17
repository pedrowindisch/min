using Min.Compiler.CodeGeneration;
using Min.Compiler.Exceptions;
using Min.Compiler.Nodes;

namespace Min.Compiler;

internal class NameAnalysis(SymbolTable symbols, List<Node> tree)
    : SemanticStep<bool>(symbols, tree), IVisitor<bool>
{
    public override bool Execute()
    {
        foreach (var node in _nodes)
            node.Accept(this);

        return true;
    }

    public bool Visit(VariableDeclarationNode node)
    {
        try
        {
            _symbols.Add(node.Name, node.VariableType);
        }
        catch (InternalCompilerException ex) when (ex.Type is CompilerExceptionType.IdentifierAlreadyDeclared)
        {
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierAlreadyDeclared, node.Name);
        }

        return default;
    }

    public bool Visit(LiteralNode node)
    {
        return default;
    }

    public bool Visit(BinaryExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);

        return default;
    }

    public bool Visit(UnaryExpressionNode node)
    {
        node.Expression.Accept(this);
        return default;
    }

    public bool Visit(VariableNode node)
    {
        if (!_symbols.IsSaved(node.Name))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Name);

        return default;
    }

    public bool Visit(GroupingNode node)
    {
        node.Expression.Accept(this);

        return default;
    }

    public bool Visit(AssignmentStatementNode node)
    {
        if (!_symbols.IsSaved(node.Identifier))
            throw new CompilerException(node.Start.Line, node.Start.Column, CompilerExceptionType.IdentifierNotDeclared, node.Identifier);

        return default;
    }

    public bool Visit(InputStatementNode node)
    {
        node.Variable.Accept(this);
        return default;
    }

    public bool Visit(OutputStatementNode node)
    {
        foreach (var value in node.Values)
            value.Accept(this);

        return default;
    }

    public bool Visit(IfStatementNode node)
    {
        node.Condition?.Accept(this);
        return default;
    }
}
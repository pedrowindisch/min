using System.Text;
using Min.Compiler.Nodes;

namespace Min.Compiler.CodeGeneration;

public class RawCilGenerator(SymbolTable symbols, ProgramNode root) : BaseCodeGenerator(symbols, root), IVisitor
{
    private readonly string HEADERS = string.Join('\n', new string[] {
        ".assembly extern mscorlib {}",
        ".assembly 'App' {}",
        ".class private auto ansi beforefieldinit abstract sealed Program extends [mscorlib]System.Object {",
        ".method private hidebysig static void Main(string[] args) cil managed {",
        ".entrypoint",
    });

    private readonly string FOOTERS = string.Join('\n', new string[] {
        "ret",
        "}",
        "}"
    });

    private readonly Dictionary<BuiltInType, string> BuiltInTypeCilMap = new()
    {
        { BuiltInType.Bool, "bool" },
        { BuiltInType.Int, "int32" },
        { BuiltInType.Float, "float32" },
        { BuiltInType.String, "string" }
    };

    private readonly Dictionary<BuiltInOperator, string[]> OperatorCilMap = new()
    {
        { BuiltInOperator.Add, ["add"] },
        { BuiltInOperator.Subtract, ["sub"] },
        { BuiltInOperator.Multiply, ["mul"] },
        { BuiltInOperator.Divide, ["div"] },
        { BuiltInOperator.EqualsTo, ["ceq"] },
        { BuiltInOperator.NotEqualsTo, ["ceq", "not"] },
        { BuiltInOperator.GreaterThan, ["cgt"] },
        { BuiltInOperator.GreaterThanOrEquals, ["clt", "not"] },
        { BuiltInOperator.LessThan, ["clt"] },
        { BuiltInOperator.LessThanOrEquals, ["cgt", "not"] }
    };

    private readonly Stack<BuiltInType> _typeStack = new();
    private readonly Stack<string> _branchStack = new();
    private readonly StringBuilder _code = new();

    public override string Execute()
    {
        _code.Append(HEADERS);

        foreach (var node in _root.Statements)
            node.Accept(this);

        _code.Append(FOOTERS);
        return _code.ToString();        
    }

    private string GenerateBranchLabel() => "b" + Guid.NewGuid().ToString().Replace("-", "");

    public void Visit(ProgramNode node)
    {
        _code.AppendLine(".entrypoint");
    }

    public void Visit(VariableDeclarationNode node)
    {
        _code.AppendLine($".locals ({BuiltInTypeCilMap[node.Type]} {node.Identifier})");

        if (node.Value is not null)
        {
            node.Value.Accept(this);
            _code.AppendLine($"stloc {node.Identifier}");
        }
    }

    public void Visit(VariableAssignmentNode node)
    {
        node.Value.Accept(this);
        _code.AppendLine($"stloc {node.Identifier}");
    }
    public void Visit(IfStatementNode node)
    {
        node.Condition.Accept(this);

        var ifStatementLabel = GenerateBranchLabel();
        var endLabel = GenerateBranchLabel();

        List<string> labels = [];
        foreach (var _ in node.ElseIfStatements)
            labels.Add(GenerateBranchLabel());

        if (node.ElseStatement is not null)
            labels.Add(GenerateBranchLabel());

        labels = [
            ifStatementLabel,
            ..labels,
            endLabel
        ];

        node.Condition.Accept(this);
        _code.AppendLine($"brfalse {labels[1]}");

        foreach (var stmt in node.Block)
            stmt.Accept(this);

        _code.AppendLine($"br {labels[^1]}");

        for (int i = 0; i < node.ElseIfStatements.Count; i++)
        {
            var elseIfStmt = node.ElseIfStatements[i];
            _code.AppendLine($"{labels[i + 1]}:");

            elseIfStmt.Condition.Accept(this);
            _code.AppendLine($"brfalse {labels[i + 2]}");

            foreach (var stmt in elseIfStmt.Block)
                stmt.Accept(this);

            _code.AppendLine($"br {labels[^1]}");
        }

        if (node.ElseStatement is not null)
        {
            _code.AppendLine($"{labels[^2]}:");

            foreach (var stmt in node.ElseStatement.Statements)
                stmt.Accept(this);
        }

        _code.AppendLine($"{labels[^1]}:");
    }

    public void Visit(ElseIfStatementNode node)
    {
        var elseBranch = _branchStack.Pop();
        var exitBranch = _branchStack.Pop();

        // _code.AppendLine($"br.s {newBranch}");
        // _code.AppendLine($"{elseBranch}:");

        node.Condition.Accept(this);
        _code.AppendLine($"brfalse.s {elseBranch}");

        foreach (var statement in node.Block)
            statement.Accept(this);

        _code.AppendLine($"br.s {exitBranch}");
        _code.AppendLine($"{elseBranch}:");
    }

    public void Visit(ElseStatementNode node)
    {
        foreach (var statement in node.Statements)
            statement.Accept(this);
    }

    public void Visit(InputStatementNode node)
    {
        _code.AppendLine("call string [mscorlib]System.Console::ReadLine()");
        _code.AppendLine($"stloc {node.Identifier}");
    }

    public void Visit(OutputStatementNode node)
    {
        foreach (var expression in node.Values)
        {
            expression.Accept(this);
            _code.AppendLine($"call void [mscorlib]System.Console::WriteLine({BuiltInTypeCilMap[_typeStack.Pop()]})");
        }
    }

    public void Visit(ComparisonExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);

        var operatorCil = OperatorCilMap[node.Operator];
        foreach (var op in operatorCil)
            _code.AppendLine(op);
    }

    public void Visit(MultiplicativeExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);

        var operatorCil = OperatorCilMap[node.Operator];
        foreach (var op in operatorCil)
            _code.AppendLine(op);
    }

    public void Visit(AdditiveExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);

        var operatorCil = OperatorCilMap[node.Operator];
        foreach (var op in operatorCil)
            _code.AppendLine(op);
    }

    public void Visit(UnaryExpressionNode node)
    {
        _typeStack.Push(BuiltInType.Int);
        _code.AppendLine($"lcd.i4 {node.Value}");
        _code.AppendLine("ldc.i4 -1");
        _code.AppendLine("mul");
    }

    public void Visit(StringExpressionNode node)
    {
        _typeStack.Push(BuiltInType.String);
        _code.AppendLine($"ldstr \"{node.Value}\"");
    }

    public void Visit(NumberExpressionNode node)
    {
        _typeStack.Push(BuiltInType.Int);
        _code.AppendLine($"ldc.i4 {node.Value}");
    }

    public void Visit(BooleanExpressionNode node)
    {
        _typeStack.Push(BuiltInType.Bool);
        _code.AppendLine($"ldc.i4 {(node.Value ? 1 : 0)}");
    }

    public void Visit(IdentifierExpressionNode node)
    {
        _typeStack.Push(_symbols.GetType(node.Identifier));
        _code.AppendLine($"ldloc {node.Identifier}");
    }

    public void Visit(GroupingExpressionNode node)
    {
        node.Value.Accept(this);
    }
}
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
    private readonly StringBuilder _code = new();

    public override string Execute()
    {
        _code.Append(HEADERS);

        foreach (var node in _root.Statements)
            node.Accept(this);

        _code.Append(FOOTERS);
        return _code.ToString();        
    }

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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
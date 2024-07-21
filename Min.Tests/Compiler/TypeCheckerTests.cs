using Min.Compiler;
using Min.Compiler.Nodes;

namespace Min.Tests.Compiler;

public class TypeCheckerTests
{
    [Fact(DisplayName = "Declaring a integer variable and assigning it a integer constant")]
    public void TypeCheck_VariableDeclaration_IntegerConstants()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.Int,
                new NumberExpressionNode(new Position(0, 0), 1)
            )
        ]));
    }

    [Fact(DisplayName = "Declaring a string variable and assinging it a string constant")]
    public void TypeCheck_VariableDeclaration_StringConstants()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.String,
                new StringExpressionNode(new Position(0, 0), "min")
            )
        ]));
    }
    
    [Theory(DisplayName = "Declaring a boolean variable and assinging it a boolean literal")]
    [InlineData(false)]
    [InlineData(true)]
    public void TypeCheck_VariableDeclaration_BooleanConstants(bool value)
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.Bool,
                new BooleanExpressionNode(new Position(0, 0), value)
            )
        ]));
    }

    [Fact(DisplayName = "Declaring a variable and assinging it another variable as value")]
    public void TypeCheck_VariableDeclaration_WithAnotherVariableAsValue()
    {
        var typeChecker = new TypeChecker();
        var symbolTable = new SymbolTable();
        symbolTable.Add(".min", BuiltInType.String);

        typeChecker.Execute(symbolTable, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.String,
                new IdentifierExpressionNode(new Position(0, 0), ".min")
            )
        ]));
    }

    [Fact(DisplayName = "Declaring a variable and assinging it another variable with a different type should thrown an exception")]
    public void TypeCheck_VariableDeclaration_WithAnotherVariableAsValueButWithWrongType_ThrowsException()
    {
        var typeChecker = new TypeChecker();
        var symbolTable = new SymbolTable();
        symbolTable.Add(".min", BuiltInType.Int);

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(symbolTable, new([
                new VariableDeclarationNode(
                    new Position(0, 0),
                    ".test",
                    BuiltInType.String,
                    new IdentifierExpressionNode(new Position(0, 0), ".min")
                )
            ]));
        });
    }

    [Fact(DisplayName = "Declaring an int variable and assigning it an expression (additive)")]
    public void TypeCheck_VariableDeclaration_AdditiveExpressionWithConstants()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.Int,
                new AdditiveExpressionNode(
                    new Position(0, 0),
                    new NumberExpressionNode(new Position(0, 0), 1),
                    BuiltInOperator.Add,
                    new NumberExpressionNode(new Position(0, 0), 1)
                )
            )
        ]));
    }

    [Fact(DisplayName = "Declaring an int variable and assigning it an expression (multiplicative)")]
    public void TypeCheck_VariableDeclaration_MultiplicativeExpressionWithConstants()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.Int,
                new MultiplicativeExpressionNode(
                    new Position(0, 0),
                    new NumberExpressionNode(new Position(0, 0), 1),
                    BuiltInOperator.Divide,
                    new NumberExpressionNode(new Position(0, 0), 1)
                )
            )
        ]));
    }

    [Fact(DisplayName = "Declaring a bool variable and assigning it a relational comparison")]
    public void TypeCheck_VariableDeclaration_RelationalComparation()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new VariableDeclarationNode(
                new Position(0, 0),
                ".test",
                BuiltInType.Bool,
                new ComparisonExpressionNode(
                    new Position(0, 0),
                    new NumberExpressionNode(new Position(0, 0), 1),
                    BuiltInOperator.EqualsTo,
                    new NumberExpressionNode(new Position(0, 0), 1)
                )
            )
        ]));
    }

    [Fact(DisplayName = "Reassigning a variable with the same type")]
    public void TypeCheck_VariableAssignment_ValueWitSameTypeAsDeclaration()
    {
        var typeChecker = new TypeChecker();
        var symbolTable = new SymbolTable();
        symbolTable.Add(".min", BuiltInType.Int);

        typeChecker.Execute(symbolTable, new([
            new VariableAssignmentNode(
                new Position(0, 0),
                ".min",
                new NumberExpressionNode(new Position(0, 0), 123)
            )
        ]));
    }

    [Fact(DisplayName = "Reassigning a variable with a value of a different type throws an exception")]
    public void TypeCheck_VariableAssignment_ValueWithDifferentType_ThrowsException()
    {
        var typeChecker = new TypeChecker();
        var symbolTable = new SymbolTable();
        symbolTable.Add(".test", BuiltInType.Int);

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(symbolTable, new([
                new VariableAssignmentNode(
                    new Position(0, 0),
                    ".test",
                    new StringExpressionNode(new Position(0, 0), "test")
                )
            ]));
        });
    }

    [Fact(DisplayName = "Declaring a variable with a type and assigning it a value of a different type throws an exception")]
    public void TypeCheck_VariableDeclaration_WrongValueType_ThrowsException()
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new VariableDeclarationNode(
                    new Position(0, 0),
                    ".test",
                    BuiltInType.String,
                    new NumberExpressionNode(new Position(0, 0), 123)
                )
            ]));
        });
    }

    [Fact(DisplayName = "If statement with a condition that results in a boolean value")]
    public void TypeCheck_IfStatement_ConditionResultingInBoolean()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new IfStatementNode(
                new Position(0, 0),
                new ComparisonExpressionNode(
                    new Position(0, 0),
                    new NumberExpressionNode(new Position(0, 0), 1),
                    BuiltInOperator.EqualsTo,
                    new NumberExpressionNode(new Position(0, 0), 1)
                ),
                []
            )
        ]));
    }

    [Fact(DisplayName = "If statement with a boolean constant as thd condition")]
    public void TypeCheck_IfStatement_ConditionWithBooleanConstant()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new IfStatementNode(
                new Position(0, 0),
                new BooleanExpressionNode(new Position(0, 0), true),
                []
            )
        ]));
    }

    [Fact(DisplayName = "If statement with condition that does not result in a boolean value should throw an exception")]
    public void TypeCheck_IfStatement_ConditionWithNonBooleanValue_ThrowsException()
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new IfStatementNode(
                    new Position(0, 0),
                    new AdditiveExpressionNode(
                        new Position(0, 0),
                        new NumberExpressionNode(new Position(0, 0), 1),
                        BuiltInOperator.Add,
                        new NumberExpressionNode(new Position(0, 0), 1)
                    ),
                    []
                )
            ]));
        });
    }

    [Theory(DisplayName = "Expression with different left and right types throws an exception (additive)")]
    [InlineData(BuiltInOperator.Add)]
    [InlineData(BuiltInOperator.Subtract)]
    public void TypeCheck_AdditiveExpression_DifferentTypes(BuiltInOperator op)
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new OutputStatementNode(
                    new Position(0,0),
                    [new AdditiveExpressionNode(
                        new Position(0, 0),
                        new NumberExpressionNode(new Position(0, 0), 1),
                        op,
                        new StringExpressionNode(new Position(0, 0), "abc")
                    )]
                )
            ]));
        });
    }

    [Theory(DisplayName = "Expression with different left and right types throws an exception (multiplicative)")]
    [InlineData(BuiltInOperator.Multiply)]
    [InlineData(BuiltInOperator.Divide)]
    public void TypeCheck_MultiplicativeExpression_DifferentTypes(BuiltInOperator op)
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new OutputStatementNode(
                    new Position(0,0),
                    [new MultiplicativeExpressionNode(
                        new Position(0, 0),
                        new NumberExpressionNode(new Position(0, 0), 1),
                        op,
                        new StringExpressionNode(new Position(0, 0), "abc")
                    )]
                )
            ]));
        });
    }

    [Theory(DisplayName = "Expression with different left and right types throws an exception (comparison)")]
    [InlineData(BuiltInOperator.EqualsTo)]
    [InlineData(BuiltInOperator.NotEqualsTo)]
    [InlineData(BuiltInOperator.GreaterThan)]
    [InlineData(BuiltInOperator.GreaterThanOrEquals)]
    [InlineData(BuiltInOperator.LessThan)]
    [InlineData(BuiltInOperator.LessThanOrEquals)]
    public void TypeCheck_ComparisonExpression_DifferentTypes(BuiltInOperator op)
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new OutputStatementNode(
                    new Position(0,0),
                    [new ComparisonExpressionNode(
                        new Position(0, 0),
                        new NumberExpressionNode(new Position(0, 0), 1),
                        op,
                        new StringExpressionNode(new Position(0, 0), "abc")
                    )]
                )
            ]));
        });
    }

    [Fact(DisplayName = "Unary expression with non-numeric values should throw an exception")]
    public void TypeCheck_UnaryExpression_NonNumericType_ThrowsException()
    {
        var typeChecker = new TypeChecker();

        Assert.ThrowsAny<Exception>(() =>
        {
            typeChecker.Execute(null!, new([
                new OutputStatementNode(
                    new Position(0,0),
                    [new UnaryExpressionNode(
                        new Position(0, 0),
                        BuiltInOperator.Subtract,
                        new StringExpressionNode(new Position(0, 0), "abc")
                    )]
                )
            ]));
        });
    }

    [Fact(DisplayName = "Unary expression with grouping that results in numeric value")]
    public void TypeCheck_UnaryExpression_GroupingThatResultsInNumericValue()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new OutputStatementNode(
                new Position(0,0),
                [new UnaryExpressionNode(
                    new Position(0, 0),
                    BuiltInOperator.Subtract,
                    new GroupingExpressionNode(
                        new Position(0, 0),
                        new AdditiveExpressionNode(
                            new Position(0, 0),
                            new NumberExpressionNode(new Position(0, 0), 123),
                            BuiltInOperator.Add,
                            new NumberExpressionNode(new Position(0, 0), 123)
                        )
                    )
                )]
            )
        ]));
    }


    [Fact(DisplayName = "Unary expression with numeric constant")]
    public void TypeCheck_UnaryExpression_NumericConstant()
    {
        var typeChecker = new TypeChecker();

        typeChecker.Execute(null!, new([
            new OutputStatementNode(
                new Position(0,0),
                [new UnaryExpressionNode(
                    new Position(0, 0),
                    BuiltInOperator.Subtract,
                    new NumberExpressionNode(new Position(0, 0), 123)
                )]
            )
        ]));
    }
}
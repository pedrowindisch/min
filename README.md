# _min_ - A small programming language
<sup>(tl;dr: go straight to [_min_ in five minutes](#min-in-five-minutes))<sup>

_min_ is a compiler for a small toy programming language that targets the .NET platform (the compiler outputs CIL code).

## Using the CLI/compiler
As no pre-built versions of the compiler are readily available to download, you must clone the project and build it yourself.

Alternatively, run the project using `dotnet run -p Min.Cli {sourceCode}`, where sourceCode is the file, written in the _min_ langugage, that you want to compile.

The compiler overwrites any existing file when saving the output code (this behavior can be turned off with the `--overwwrite` flag, by passing it a value of `false`).

To list all the options of the compiler, pass the flag `-h`, or `--help`, when running it.

## Key objectives
The main objective of this project was to create a compiler for a simple programming language. I've placed an emphasis on developing a reliable tokenizer and parser error reporting system, with user-friendly error messages.

The _min_ compiler outputs CIL (Common Intermediate Language) code, but the project structure allows for easy extensibility to new output targets.

## Project structure (overview)
```
Min.sln
├── Min/
|   ├── Min.cs
│   └── Compiler/
|       ├── SemanticAnalysisPipeline.cs
|       ├── Nodes/
|       ├── Exceptions/
|           ├── CompilerExceptionType.cs
|           ├── CompilerException.cs
|           └── InternalCompilerException.cs
│       └── CodeGeneration/
│   
├── Min.Cli/
└── Min.Tests/
```

The _min_ solution contains three different projects. The compiler code itself is located at the `Min` project.

#### Errors
Compiler errors should be raised with either the `CompilerException` or the `InternalCompilerException` class. Both classes must be provided with an exception type. New error types must be configured in the `CompilerExceptionType.cs` file.

Internal compiler exceptions are not supposed to be propagated to the end user.

The compiler itself does not handle error formatting and printing (that is, the syntax and line highlighting), it just generates the message that will be propagated to the end user. The error formatting is handled in the `Min.Cli` project.

#### Extending the code generation step
A code generation target can be created by inheriting the `BaseCodeGenerator` class. There's currently no way to dinamically select the code generation target from the CLI.

#### Semantic analysis steps
Currently, there are two active semantic analysis steps: the name analysis and the type checking step.

New steps can be created by implementing the interface `ISemanticAnalysisStep`.

## Language specification
The _min_ syntax is intentionally small. For a detailed specification, please refer to [SPECS.md](SPECS.md), [samples](Min.Tests/Samples/ValidSamples/) folder inside the compiler tests project or the eBNF grammar in [min.ebnf](min.ebnf).

## _min_ in five minutes
```
# Variables must be declared with a dot, followed by a sequence of alphanumeric chars and their type.
# Optionally, variables can also include their initial value.
.year int
.year int = 2024
.name string = "min"

# Conditionals
if .year < 2000:
  print "before 2000"
else if .year < 2010:
  print "before 2010"
else:
  print "2010 or after"
endif

# Input and output (with aliases)
input .string
output .string
in .string
out .string

# Functions (proposal)
.sayHello (
    .name string
    .uppercase boolean
):
  if .uppercase:
    output "HELLO ", upper .name # upper is a built-in function
  else:
    output "Hello ", .name
  endif
endfunc

# Calling functions
.sayHello "min", true # This should output "HELLO MIN"

# Iteration structures (proposal)
while .year >= 2000:
  .year = .year = 1
end
```

# _min_ - A small programming language
<sup>(tl;dr: go straight to [_min_ in five minutes](#min-in-five-minutes))<sup>

## Key objectives
The main objective of this project is to create a simple programming language, primarily for educational purposes and to delve into the process of building a compiler. The project places a emphasis on developing a reliable tokenizer and parser error reporting system, with user-friendly error messages.

The _min_ compiler outputs CIL (Common Intermediate Language) code.

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

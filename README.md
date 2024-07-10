# _min_ - A small programming language
## Key objectives
The primary goal of this project is to create a simple programming language, mainly for educational purposes and to explore the aspects of building a compiler, with a strong focus on developing a robust tokenizer and parser error reporting system.

> [!CAUTION]
> This project was made as a learning project, so sorry in advance for the possibly messy codebase.

## Language specification
The _min_ syntax is intentionally small. For a detailed specification, please refer to [SPECS.md](SPECS.md).

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
end

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
    print "HELLO ", upper .name # upper is a built-in function
  else:
    print "Hello ", .name
  end
end

# Calling functions
.sayHello "min", true # This should output "HELLO MIN"

# Iteration structures (proposal)
while .year >= 2000:
  .year = .year = 1
end
```

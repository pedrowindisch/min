using System.Collections;
using Min.Compiler.Exceptions;

namespace Min.Compiler;

internal class Tokenizer : IEnumerable<Token>
{
    private static readonly Dictionary<string, TokenType> OPERATORS = new()
    {
        { "=", TokenType.Assign },
        { "==", TokenType.EqualsTo },
        { "!=", TokenType.NotEqualsTo },
        { ">=", TokenType.GreaterThanOrEqual },
        { ">", TokenType.GreaterThan },
        { "<=", TokenType.LessThanOrEqual },
        { "<", TokenType.LessThan },
        { "*", TokenType.Multiply },
        { "+", TokenType.Add },
        { "-", TokenType.Subtract },
        { "/", TokenType.Divide },
    };

    private static readonly Dictionary<string, TokenType> KEYWORDS = new()
    {
        { "int", TokenType.Int },
        { "float", TokenType.Float },
        { "string", TokenType.String },
        { "bool", TokenType.Bool },
        { "input", TokenType.Input },
        { "in", TokenType.Input },
        { "output", TokenType.Output },
        { "out", TokenType.Output },
        { "if", TokenType.If },
        { "else", TokenType.Else },
        { "endif", TokenType.EndIf },
        { "true", TokenType.True },
        { "false", TokenType.False }
    };

    private string _source;
    private int _currentLine = 1;
    private int _currentColumn = 0;
    private int _start = 0;
    private int _index = 0;

    public Tokenizer(string source)
    {
        _source = source;
    }

    private char Peek(int positions = 0) => _source.ElementAtOrDefault(_index + positions);
    private bool IsAtEnd() => _index >= _source.Length;

    public IEnumerator<Token> GetEnumerator()
    {
        while (!IsAtEnd())
        {
            IgnoreWhitespaceAndComments();
            if (IsAtEnd()) break;

            _start = _currentColumn;

            char currentChar = Peek();
            var token = currentChar switch
            {
                '.' => MatchIdentifier(),
                char when char.IsLetter(currentChar) => MatchKeyword(),
                '<' or '>' or '=' or '!' or '+' or '-' or '*' or '/' => MatchOperator(),
                char when char.IsNumber(currentChar) => MatchNumber(),
                '"' => MatchString(),
                ',' or ':' or '(' or ')' => MatchPunctuation(),

                _ => throw new CompilerException(_currentLine, _currentColumn, CompilerExceptionType.UnexpectedCharacter, currentChar)
            };

            yield return token;
        }

        yield return new Token(_currentLine, _currentColumn, TokenType.EOF);
    }

    private Token MatchPunctuation()
    {
        var token = new Token(
            _currentLine, 
            _start, 
            Peek() switch
            {
                ',' => TokenType.Comma,
                ':' => TokenType.Colon,
                '(' => TokenType.LeftParenthesis,
                ')' => TokenType.RightParenthesis,

                _ => throw new Exception("should not be called")
            });     

        TakeChar();
        return token;   
    }

    private Token MatchOperator()
    {
        var op = TakeWhile(ch => ch is '<' or '>' or '=' or '!' or '+' or '-' or '*' or '/');

        if (!OPERATORS.TryGetValue(op, out var type))
            throw new CompilerException(_currentLine, _start, CompilerExceptionType.UnrecognizedOperator, op);

        return new Token(_currentLine, _start, type);        
    }

    private Token MatchString()
    {
        TakeChar();
        var value = TakeWhile(ch => ch is not '"');

        // TakeWhile ignores the end of the file.
        if (TakeChar() is not '"')
            throw new CompilerException(_currentLine, _currentColumn, CompilerExceptionType.UnterminatedString);

        return new Token(_currentLine, _start, TokenType.StringLiteral, value);
    }

    private Token MatchNumber()
    {
        var number = TakeWhile(char.IsNumber);
        while (Peek() is '.')
        {
            if (!char.IsNumber(Peek(1)))
                throw new CompilerException(_currentLine, _start, CompilerExceptionType.InvalidNumberLiteral, "Numbers literals cannot end with a dot, either finish it with '.0' or, if you're trying to declare a variable, add a space before the dot.");

            number += TakeChar();
            number += TakeWhile(char.IsNumber);
        }

        return new Token(_currentLine, _start, TokenType.NumberLiteral, number);
    }

    private Token MatchKeyword()
    {
        var value = TakeWhile(char.IsLetter);
        if (!KEYWORDS.TryGetValue(value, out var keyword))
            throw new CompilerException(_currentLine, _start, CompilerExceptionType.UnrecognizedKeyword);

        return new Token(_currentLine, _start, keyword);
    }

    private Token MatchIdentifier()
    {
        if (!char.IsLetterOrDigit(Peek(1)))
            throw new CompilerException(_currentLine, _currentColumn, CompilerExceptionType.InvalidIdentifier, "Identifiers must start with a letter or digit.");

        TakeChar();
        var identifier = TakeWhile(ch =>
        {
            if (char.IsLetterOrDigit(ch)) return true;
            if (char.IsWhiteSpace(ch) || ch is ',') return false;

            throw new CompilerException(_currentLine, _start, CompilerExceptionType.InvalidIdentifier, "Identifiers should only contain letters and numbers.");
        });

        return new Token(_currentLine, _start, TokenType.Identifier, identifier);
    }

    private void IgnoreWhitespaceAndComments()
    {
        IgnoreWhitespace();
        IgnoreComments();
    }

    private void IgnoreWhitespace() => TakeWhile(char.IsWhiteSpace);
    private void IgnoreComments()
    {
        while (Peek() is '#')
        {
            TakeChar();

            var line = _currentLine;
            TakeWhile(_ => line == _currentLine);
        }
    }

    private string TakeWhile(Func<char, bool> condition)
    {
        var result = "";
        while (!IsAtEnd())
        {
            char currentChar = Peek();
            if (!condition(currentChar))
                break;

            result += TakeChar();
        }

        return result;
    }

    private char TakeChar()
    {
        char currentChar = Peek();
        _currentColumn++;
        _index++;

        if (currentChar is '\n')
            AdvanceLine();

        return currentChar;
    }

    private void AdvanceLine() 
    {
        _currentLine += 1;
        _currentColumn = 0;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
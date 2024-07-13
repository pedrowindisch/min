using System.Collections;
using Min.Compiler.Exceptions;

namespace Min.Compiler;

public class Tokenizer : IEnumerable<Token>
{
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

    private char Peek(int positions = 0) => IsAtEnd() ? '\0' : _source[_index + positions];
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
                '.' when char.IsLetterOrDigit(Peek(1)) => MatchIdentifier(),
                '.' => throw new Exception("Expected an identifier. Identifiers start with a dot, followed by a sequence of alphanumeric characters."),
                char when char.IsLetter(currentChar) => MatchKeyword(),

                // '>' or '=' or '!' or '+' or '-' or '*' or '/' => MatchOperator(),
                char when char.IsNumber(currentChar) => MatchNumber(),
                '"' => MatchString(),

                _ => throw new CompilerException(_currentLine, _currentColumn, CompilerExceptionMessages.UnexpectedCharacter(currentChar))
            };

            yield return token;
        }

        yield return new Token(_currentLine, _currentColumn, TokenType.EOF);
    }

    private Token MatchString()
    {
        TakeChar();
        var value = TakeWhile(ch => ch is not '"');

        // TakeWhile ignores the end of the file.
        if (TakeChar() is not '"')
            throw new CompilerException(_currentLine, _currentColumn, CompilerExceptionMessages.UnterminatedString());

        return new Token(_currentLine, _start, TokenType.StringLiteral, value);
    }

    private Token MatchNumber()
    {
        var number = TakeWhile(char.IsNumber);
        while (Peek() is '.')
        {
            if (!char.IsNumber(Peek(1)))
                throw new Exception();

            number += TakeChar();
            number += TakeWhile(char.IsNumber);
        }

        return new Token(_currentLine, _start, TokenType.NumberLiteral, number);
    }

    private Token MatchKeyword()
    {
        var value = TakeWhile(char.IsLetter);
        if (!KEYWORDS.TryGetValue(value, out var keyword))
            throw new Exception();

        return new Token(_currentLine, _start, keyword);
    }

    private Token MatchIdentifier()
    {
        TakeChar();
        var identifier = TakeWhile(ch =>
        {
            if (char.IsLetterOrDigit(ch)) return true;
            if (char.IsWhiteSpace(ch)) return false;

            throw new Exception("Unexpected character");
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
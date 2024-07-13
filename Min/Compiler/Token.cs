namespace Min.Compiler;

public class Token
{
    public int Line { get; }
    public int Column { get; }
    public TokenType Type { get; }
    public string? Lexeme { get; }

    public Token(int line, int column, TokenType type)
    {
        Line = line;
        Column = column;
        Type = type;
    }

    public Token(int line, int column, TokenType type, string lexeme) : this(line, column, type)
    {
        Lexeme = lexeme;
    }
}
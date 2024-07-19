namespace Min.Compiler;

public record Position(int Line, int Column)
{
    public static Position From(Token token) => new(token.Line, token.Column);
};
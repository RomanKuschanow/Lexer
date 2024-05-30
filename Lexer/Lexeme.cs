namespace Lexer;
public record Lexeme
{
    public string Type { get; }

    public string Value { get; }

    public Lexeme(string type, string value)
    {
        Type = type; 
        Value = value;
    }

    public override string ToString() => Value;
}

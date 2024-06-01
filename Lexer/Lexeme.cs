namespace Lexer;
/// <summary>
/// Represents a lexeme, an identified segment of text according to a rule.
/// </summary>
public record Lexeme
{
    /// <summary>
    /// Gets the type of the lexeme, corresponding to the name of the rule that identified it.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the string value of the lexeme found in the text.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Lexeme"/> record.
    /// </summary>
    /// <param name="type">The type of the lexeme, typically corresponding to the rule name.</param>
    /// <param name="value">The string value of the lexeme found.</param>
    public Lexeme(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => Value;
}


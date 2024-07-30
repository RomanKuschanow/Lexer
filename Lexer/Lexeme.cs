namespace Lexer;
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
    /// Gets the int value of the lexeme start index.
    /// </summary>
    public int StartIndex { get; }

    /// <summary>
    /// Gets the int value of the lexeme length.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Lexeme"/> record.
    /// </summary>
    /// <param name="type">The type of the lexeme, typically corresponding to the rule name.</param>
    /// <param name="value">The string value of the lexeme found.</param>
    public Lexeme(string type, string value, int startIndex, int length)
    {
        Type = type;
        Value = value;
        StartIndex = startIndex;
        Length = length;
    }

    public override string ToString() => Value;
}


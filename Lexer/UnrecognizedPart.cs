namespace Lexer;
public record UnrecognizedPart
{
    /// <summary>
    /// Gets the line number where the unrecognized text segment begins.
    /// </summary>
    public int Ln { get; }

    /// <summary>
    /// Gets the character position in the line where the unrecognized text segment begins.
    /// </summary>
    public int Ch { get; }

    /// <summary>
    /// Gets the length of the unrecognized text segment.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnrecognizedPart"/> record.
    /// </summary>
    /// <param name="ln">The line number where the unrecognized text segment begins.</param>
    /// <param name="ch">The character position in the line where the unrecognized text segment begins.</param>
    /// <param name="length">The length of the unrecognized text segment.</param>
    public UnrecognizedPart(int ln, int ch, int length)
    {
        Ln = ln;
        Ch = ch;
        Length = length;
    }
}


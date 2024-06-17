using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults;
/// <summary>
/// Represents a raw lexeme identified in the text before final processing.
/// This includes its position, length, and the rule used to identify it.
/// </summary>
public record RawLexeme
{
    /// <summary>
    /// Gets the starting position of the lexeme within the text.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the length of the lexeme within the text.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the rule used to identify this lexeme.
    /// </summary>
    public IRule Rule { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RawLexeme"/> record.
    /// </summary>
    /// <param name="start">The starting position of the lexeme within the text.</param>
    /// <param name="length">The length of the lexeme within the text.</param>
    /// <param name="rule">The rule used to identify this lexeme.</param>
    public RawLexeme(int start, int length, IRule rule)
    {
        Start = start;
        Length = length;
        Rule = rule;
    }
}


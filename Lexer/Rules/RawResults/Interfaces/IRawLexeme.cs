using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLexeme
{
    /// <summary>
    /// Gets the start position of the lexeme.
    /// </summary>
    int Start { get; }

    /// <summary>
    /// Gets the length of the lexeme.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets the rule associated with the lexeme.
    /// </summary>
    IRule Rule { get; }

    /// <summary>
    /// Gets the type of the lexeme.
    /// </summary>
    string Type { get; }
}

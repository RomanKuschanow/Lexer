using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayer
{
    /// <summary>
    /// Gets the collection of raw lexemes in the layer.
    /// </summary>
    public IEnumerable<IRawLexeme> RawLexemes { get; }

    /// <summary>
    /// Gets the rule associated with the raw layer.
    /// </summary>
    public IRule Rule { get; }
}

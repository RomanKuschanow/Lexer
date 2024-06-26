using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayerCreator
{
    /// <summary>
    /// Creates a raw layer using the specified raw lexemes and rule.
    /// </summary>
    /// <param name="rawLexemes">The collection of raw lexemes.</param>
    /// <param name="rule">The rule associated with the raw layer.</param>
    /// <returns>The created raw layer.</returns>
    IRawLayer Create(IEnumerable<IRawLexeme> rawLexemes, IRule rule);
}

using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayerFactory : IDisposable
{
    /// <summary>
    /// Gets the collection of raw layer creators.
    /// </summary>
    IEnumerable<IRawLayerCreator> RawLayerCreators { get; }

    /// <summary>
    /// Creates a raw layer using the specified creator type, raw lexemes, and rule.
    /// </summary>
    /// <param name="creatorType">The type of the raw layer creator.</param>
    /// <param name="rawLexemes">The collection of raw lexemes.</param>
    /// <param name="rule">The rule associated with the <paramref name="creatorType"/>.</param>
    /// <returns>The created raw layer.</returns>
    public IRawLayer CreateRawLayer(Type creatorType, IEnumerable<IRawLexeme> rawLexemes, IRule rule);
}

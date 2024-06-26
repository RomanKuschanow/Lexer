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
    /// <param name="rule">The target rule.</param>
    /// <param name="rawLexemes">The collection of raw lexemes.</param>
    /// <returns>The created raw layer.</returns>
    public IRawLayer CreateRawLayer(IRule rule, IEnumerable<IRawLexeme> rawLexemes);
}

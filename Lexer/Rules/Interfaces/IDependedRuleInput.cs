using Lexer.Rules.RawResults;
using System.Collections.Immutable;

namespace Lexer.Rules.Interfaces;
public interface IDependedRuleInput : IRuleInput
{
    /// <summary>
    /// Gets the list of analyzed layer dependencies.
    /// </summary>
    ImmutableDictionary<IRule, AnalyzedLayer> Dependencies { get; }
}


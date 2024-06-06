using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs;
public interface IDependencyRuleInput : IRuleInput
{
    /// <summary>
    /// Gets the list of analyzed layer dependencies.
    /// </summary>
    ImmutableDictionary<IRule<IRuleInput>, AnalyzedLayer> Dependencies { get; }
}


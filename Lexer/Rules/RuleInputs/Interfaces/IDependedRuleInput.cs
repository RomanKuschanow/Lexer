using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IDependedRuleInput : IRuleInput
{
    /// <summary>
    /// Gets the list of analyzed layer dependencies.
    /// </summary>
    ImmutableDictionary<IRule, IRawLayer> Dependencies { get; }
}


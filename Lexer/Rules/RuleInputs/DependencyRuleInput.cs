using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs;
public class DependencyRuleInput : RuleInput, IDependedRuleInput
{
    public ImmutableDictionary<IRule<IRuleInput>, AnalyzedLayer> Dependencies { get; init; }

    public DependencyRuleInput(string text, IDictionary<IRule<IRuleInput>, AnalyzedLayer> dependencies) : base(text)
    {
        Dependencies = dependencies.ToImmutableDictionary();
    }
}

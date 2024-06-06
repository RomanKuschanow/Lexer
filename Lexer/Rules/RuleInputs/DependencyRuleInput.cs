using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs;
public class DependencyRuleInput : RuleInput, IDependencyRuleInput
{
    public ImmutableDictionary<IRule, AnalyzedLayer> Dependencies { get; init; }

    public DependencyRuleInput(string text, IDictionary<IRule, AnalyzedLayer> dependencies) : base(text)
    {
        Dependencies = dependencies.ToImmutableDictionary();
    }
}

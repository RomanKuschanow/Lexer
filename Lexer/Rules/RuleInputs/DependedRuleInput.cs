using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs;
public class DependedRuleInput : RuleInput, IDependedRuleInput
{
    public ImmutableDictionary<IRule, AnalyzedLayer> Dependencies { get; init; }

    public DependedRuleInput(string text, IDictionary<IRule, AnalyzedLayer> dependencies) : base(text)
    {
        Dependencies = dependencies.ToImmutableDictionary();
    }
}

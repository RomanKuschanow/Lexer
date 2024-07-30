using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs;
public class DependedRuleInput : CommonRuleInput, IDependedRuleInput
{
    public ImmutableDictionary<IRule, IRawLayer> Dependencies { get; init; }

    public DependedRuleInput(string text, IDictionary<IRule, IRawLayer> dependencies) : base(text)
    {
        Dependencies = dependencies.ToImmutableDictionary();
    }
}

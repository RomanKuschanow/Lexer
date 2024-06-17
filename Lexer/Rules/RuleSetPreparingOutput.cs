using Lexer.Rules.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules;
public class RuleSetPreparingOutput
{
    public ImmutableList<KeyValuePair<IDependedRule, IRule>> RemovedDependencies { get; init; }

    public RuleSetPreparingOutput(IEnumerable<KeyValuePair<IDependedRule, IRule>> removedDependencies)
    {
        RemovedDependencies = removedDependencies.ToImmutableList();
    }
}

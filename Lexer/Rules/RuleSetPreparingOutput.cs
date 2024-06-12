using Lexer.Rules.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules;
public class RuleSetPreparingOutput
{
    public ImmutableList<KeyValuePair<IDependencyRule, IRule>> RemovedDependencies { get; init; }

    public RuleSetPreparingOutput(IEnumerable<KeyValuePair<IDependencyRule, IRule>> removedDependencies)
    {
        RemovedDependencies = removedDependencies.ToImmutableList();
    }
}

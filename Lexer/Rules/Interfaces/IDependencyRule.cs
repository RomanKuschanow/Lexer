using Lexer.Rules.RuleInputs;
using System.Collections.Immutable;

namespace Lexer.Rules.Interfaces;
public interface IDependencyRule : IRule
{
    ImmutableList<IRule> Dependencies { get; }

    void AddDependency(IRule rule);

    void RemoveDependency(IRule rule);

    void ClearDependencies();
}

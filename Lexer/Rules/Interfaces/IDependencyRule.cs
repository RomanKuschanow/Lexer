using Lexer.Rules.RuleInputs;
using System.Collections.Immutable;

namespace Lexer.Rules.Interfaces;
public interface IDependencyRule : IRule<IDependencyRuleInput>
{
    ImmutableList<IRule<IRuleInput>> Dependencies { get; }

    void AddDependency(IRule<IRuleInput> rule);

    void RemoveDependency(IRule<IRuleInput> rule);

    void ClearDependencies();
}

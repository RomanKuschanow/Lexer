using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IDependedRule : IRule
{
    ImmutableDictionary<IRule, string[]> Dependencies { get; }

    void AddDependency(IRule rule, params string[] names);

    void SetDependencyNames(IRule rule, params string[] names);

    void RemoveDependency(IRule rule);

    void ClearDependencies();

    IEnumerable<IRawLexeme> FindLexemes(IDependedRuleInput input);
}

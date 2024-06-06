using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
using System.Collections.Immutable;

namespace Lexer.Rules;
public abstract class DependencyRuleBase : RuleBase, IDependencyRule
{
    private readonly List<IRule> _dependencies = new();

    public ImmutableList<IRule> Dependencies => _dependencies.ToImmutableList();

    public DependencyRuleBase(string type, bool isIgnored = false, bool isEnabled = true) : base(type, isIgnored, isEnabled) { }

    public void AddDependency(IRule rule)
    {
        if (_dependencies.Contains(rule)) return;
        _dependencies.Add(rule);
    }

    public void RemoveDependency(IRule rule) => _dependencies.Remove(rule);

    public void ClearDependencies() => _dependencies.Clear();
}

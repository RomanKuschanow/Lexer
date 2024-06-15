#nullable disable
using Lexer;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.Visitors;
using System.Collections.Immutable;

namespace Lexer.Rules.Depended;
public abstract class DependedRuleBase : RuleBase, IDependedRule
{
    private readonly Dictionary<IRule, string[]> _dependencies = new();

    public ImmutableDictionary<IRule, string[]> Dependencies => _dependencies.ToImmutableDictionary();

    protected DependedRuleBase(IRuleSettings ruleSettings) : base(ruleSettings) { }

    public void AddDependency(IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (_dependencies.ContainsKey(rule))
            throw new ArgumentException("\"Dependencies\" already contains specified rule", nameof(rule));
        _dependencies.Add(rule, names);
    }

    public void SetDependencyNames(IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (!_dependencies.ContainsKey(rule))
            throw new ArgumentException("\"Dependencies\" do not contain the specified rule", nameof(rule));
        _dependencies[rule] = names;
    }

    public void RemoveDependency(IRule rule) => _dependencies.Remove(rule);

    public void ClearDependencies() => _dependencies.Clear();

    public new IRuleInput Accept(IVisitor visitor, VisitorInput visitorInput) => visitor.DependencyRule(visitorInput, this);
}

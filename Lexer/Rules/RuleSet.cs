using Lexer.Extensions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet : IDisposable
{
    private readonly List<IRule> _rules = [];

    public IEnumerable<IRule> Rules => _rules.Where(r => r.IsEnabled).Distinct();

    public RuleSet() { }

    public RuleSet(IEnumerable<IRule> rules) => _rules = rules.ToList();

    public void PrepareRules()
    {
        foreach (IDependedRule rule in Rules.Where(r => r is IDependedRule).Cast<IDependedRule>())
        {
            int ruleIndex = _rules.IndexOf(rule);
            foreach (IRule dependency in rule.Dependencies.Select(d => d.Key))
            {
                if (!Rules.Contains(dependency) || _rules.IndexOf(dependency) > ruleIndex)
                    RemoveDependency(rule, dependency);
            }
        }
    }

    public void AddDependency(IDependedRule to, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(rule);

        if (!Rules.Contains(to)) throw new ArgumentException($"'{nameof(to)}' must be in current RuleSet", nameof(to));
        if (!Rules.Contains(rule)) throw new ArgumentException($"'{nameof(rule)}' must be in current RuleSet", nameof(rule));

        to.AddDependency(rule);
    }
    public void RemoveDependency(IDependedRule to, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(rule);

        if (!Rules.Contains(to)) throw new ArgumentException($"'{nameof(to)}' must be in current RuleSet", nameof(to));

        to.RemoveDependency(rule);
    }
    public void ClearDependencies(IDependedRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (!Rules.Contains(rule)) throw new ArgumentException($"'{nameof(rule)}' must be in current RuleSet", nameof(rule));

        rule.ClearDependencies();
    }

    public void Add(IRule rule)
    {
        if (!_rules.Contains(rule))
            throw new ArgumentException($"Current RuleSet already contains '{nameof(rule)}'", nameof(rule));

        _rules.Add(rule);
    }
    public void AddRange(IEnumerable<IRule> rules) => rules?.ForEach(Add);
    public void Insert(int index, IRule rule)
    {
        if (!_rules.Contains(rule))
            throw new ArgumentException($"Current RuleSet already contains '{nameof(rule)}'", nameof(rule));

        _rules.Insert(index, rule);
    }
    public bool Remove(IRule item) => _rules.Remove(item);
    public void RemoveRange(IEnumerable<IRule> rules) => rules?.ForEach(rule => Remove(rule));
    public void Clear() => _rules.Clear();

    public void Dispose() => GC.SuppressFinalize(this);
}

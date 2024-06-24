using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet
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

        if (!Rules.Contains(to)) throw new ArgumentException("'to' must be in current RuleSet", nameof(to));
        if (!Rules.Contains(rule)) throw new ArgumentException("'rule' must be in current RuleSet", nameof(rule));

        to.AddDependency(rule);
    }
    public void RemoveDependency(IDependedRule to, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(rule);

        if (!Rules.Contains(to)) throw new ArgumentException("'to' must be in current RuleSet", nameof(to));

        to.RemoveDependency(rule);
    }
    public void ClearDependencies(IDependedRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (!Rules.Contains(rule)) throw new ArgumentException("'rule' must be in current RuleSet", nameof(rule));

        rule.ClearDependencies();
    }

    public void Add(IRule rule) => _rules.Add(rule);
    public void Insert(int index, IRule rule) => _rules.Insert(index, rule);
    public void Clear() => _rules.Clear();
    public void RemoveAt(int index) => _rules.RemoveAt(index);
    public bool Remove(IRule item) => _rules.Remove(item);
}

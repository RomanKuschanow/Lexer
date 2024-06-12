using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet : IEnumerable<IRule>
{
    private readonly List<IRule> _rules = new();

    public int Count => _rules.Count;

    public IRule this[int index]
    {
        get => _rules[index];
        set => _rules[index] = value;
    }

    public RuleSet()
    {

    }

    public RuleSet(IEnumerable<IRule> rules) => _rules = rules.ToList();

    public async Task<RuleSetPreparingOutput> PrepareRules()
    {
        List<KeyValuePair<IDependencyRule, IRule>> removedDependencies = new();

        await Task.Run(() =>
        {
            foreach (IDependencyRule rule in this.Where(r => r is IDependencyRule).Cast<IDependencyRule>())
            {
                int ruleIndex = _rules.IndexOf(rule);
                foreach (IRule dependency in rule.Dependencies.Keys)
                {
                    if (!this.Contains(dependency) || _rules.IndexOf(dependency) > ruleIndex)
                    {
                        RemoveDependency(rule, dependency);
                        removedDependencies.Add(new(rule, dependency));
                    }
                }
            }
        });

        return await Task.FromResult(new RuleSetPreparingOutput(removedDependencies));
    }

    public void AddDependency(IDependencyRule to, IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(to, nameof(to));
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (!this.Contains(to)) throw new ArgumentException("\"to\" must be in this RuleSet", nameof(to));
        if (!this.Contains(rule)) throw new ArgumentException("\"rule\" must be in this RuleSet", nameof(rule));

        to.AddDependency(rule, names);
    }
    public void SetDependencyNames(IDependencyRule to, IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(to, nameof(to));
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (!this.Contains(to)) throw new ArgumentException("\"to\" must be in this RuleSet", nameof(to));
        if (!this.Contains(rule)) throw new ArgumentException("\"rule\" must be in this RuleSet", nameof(rule));

        to.SetDependencyNames(rule, names);
    }
    public void RemoveDependency(IDependencyRule to, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(to, nameof(to));
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));

        if (!this.Contains(to)) throw new ArgumentException("\"to\" must be in this RuleSet", nameof(to));

        to.RemoveDependency(rule);
    }
    public void ClearDependencies(IDependencyRule rule) => rule.ClearDependencies();

    public void Add(IRule rule) => _rules.Add(rule);
    public void Insert(int index, IRule rule) => _rules.Insert(index, rule);
    public void Clear() => _rules.Clear();
    public void RemoveAt(int index) => _rules.RemoveAt(index);
    public bool Remove(IRule item) => _rules.Remove(item);

    public IEnumerator<IRule> GetEnumerator() => _rules.Where(r => r.IsEnabled).Distinct().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

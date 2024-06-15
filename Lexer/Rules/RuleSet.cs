using Lexer.Rules.Interfaces;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet : IEnumerable<IRule<IRuleInput>>
{
    private readonly List<IRule<IRuleInput>> _rules = new();

    public int Count => _rules.Count;

    public IRule<IRuleInput> this[int index]
    {
        get => _rules[index];
        set => _rules[index] = value;
    }

    public RuleSet()
    {

    }

    public RuleSet(IEnumerable<IRule<IRuleInput>> rules) => _rules = rules.ToList();

    public async Task PrepareRules()
    {
        await Task.Run(() =>
        {
            foreach (IDependedRule rule in this.Where(r => r is IDependedRule).Cast<IDependedRule>())
            {
                int ruleIndex = _rules.IndexOf((IRule<IRuleInput>)rule);
                foreach (IRule<IRuleInput> dependency in rule.Dependencies)
                {
                    if (!this.Contains(dependency) || _rules.IndexOf(dependency) > ruleIndex)
                        RemoveDependency(rule, dependency);
                }
            }
        });
    }

    public void AddDependency(IDependedRule to, IRule<IRuleInput> rule)
    {
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(rule);

        if (!this.Contains((IRule<IRuleInput>)to)) throw new ArgumentException("\"to\" must be in this RuleSet", nameof(to));
        if (!this.Contains(rule)) throw new ArgumentException("\"rule\" must be in this RuleSet", nameof(rule));

        to.AddDependency(rule);
    }
    public void RemoveDependency(IDependedRule to, IRule<IRuleInput> rule)
    {
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(rule);

        if (!this.Contains((IRule<IRuleInput>)to)) throw new ArgumentException("\"to\" must be in this RuleSet", nameof(to));

        to.RemoveDependency(rule);
    }
    public void ClearDependencies(IDependedRule rule) => rule.ClearDependencies();

    public void Add(IRule<IRuleInput> rule) => _rules.Add(rule);
    public void Insert(int index, IRule<IRuleInput> rule) => _rules.Insert(index, rule);
    public void Clear() => _rules.Clear();
    public void RemoveAt(int index) => _rules.RemoveAt(index);
    public bool Remove(IRule<IRuleInput> item) => _rules.Remove(item);

    public IEnumerator<IRule<IRuleInput>> GetEnumerator() => _rules.Where(r => r.IsEnabled).Distinct().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

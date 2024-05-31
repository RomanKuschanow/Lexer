using System.Collections;
using System.ComponentModel;

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

    public void Add(IRule rule) => _rules.Add(rule);
    public void Insert(int index, IRule rule) => _rules.Insert(index, rule);
    public void Clear() => _rules.Clear();
    public void RemoveAt(int index) => _rules.RemoveAt(index);
    public bool Remove(IRule item) => _rules.Remove(item);

    public IEnumerator<IRule> GetEnumerator() => _rules.Where(r => r.IsEnabled).Distinct().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

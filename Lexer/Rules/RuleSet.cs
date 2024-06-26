using Lexer.Extensions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet : IRuleSet
{
    private readonly List<IRule> _rules = [];

    public IEnumerable<IRule> Rules => _rules.Where(r => r.IsEnabled).Distinct();

    public RuleSet() { }

    public RuleSet(IEnumerable<IRule> rules) => _rules = rules.ToList();

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

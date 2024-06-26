using Lexer.Extensions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections;

namespace Lexer.Rules;
public class RuleSet : IRuleSet
{
    /// <summary>
    /// List to store rules.
    /// </summary>
    private readonly List<IRule> _rules = new();

    /// <summary>
    /// Gets the collection of enabled and distinct rules.
    /// </summary>
    public IEnumerable<IRule> Rules => _rules.Where(r => r.IsEnabled).Distinct();

    /// <summary>
    /// Initializes a new instance of the <see cref="RuleSet"/> class.
    /// </summary>
    public RuleSet() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuleSet"/> class with the specified rules.
    /// </summary>
    /// <param name="rules">The initial collection of rules.</param>
    public RuleSet(IEnumerable<IRule> rules) => _rules = rules.ToList();

    /// <summary>
    /// Adds a rule to the rule set.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    /// <exception cref="ArgumentException">Thrown when the rule set already contains the specified rule.</exception>
    public void Add(IRule rule)
    {
        if (_rules.Contains(rule))
            throw new ArgumentException($"Current RuleSet already contains '{nameof(rule)}'", nameof(rule));

        _rules.Add(rule);
    }

    /// <summary>
    /// Adds a range of rules to the rule set.
    /// </summary>
    /// <param name="rules">The collection of rules to add.</param>
    public void AddRange(IEnumerable<IRule> rules) => rules?.ForEach(Add);

    /// <summary>
    /// Inserts a rule at the specified index in the rule set.
    /// </summary>
    /// <param name="index">The index at which to insert the rule.</param>
    /// <param name="rule">The rule to insert.</param>
    /// <exception cref="ArgumentException">Thrown when the rule set already contains the specified rule.</exception>
    public void Insert(int index, IRule rule)
    {
        if (_rules.Contains(rule))
            throw new ArgumentException($"Current RuleSet already contains '{nameof(rule)}'", nameof(rule));

        _rules.Insert(index, rule);
    }

    /// <summary>
    /// Removes the specified rule from the rule set.
    /// </summary>
    /// <param name="item">The rule to remove.</param>
    /// <returns><c>true</c> if the rule was removed successfully; otherwise, <c>false</c>.</returns>
    public bool Remove(IRule item) => _rules.Remove(item);

    /// <summary>
    /// Removes a range of rules from the rule set.
    /// </summary>
    /// <param name="rules">The collection of rules to remove.</param>
    public void RemoveRange(IEnumerable<IRule> rules) => rules?.ForEach(rule => Remove(rule));

    /// <summary>
    /// Clears all rules from the rule set.
    /// </summary>
    public void Clear() => _rules.Clear();

    /// <summary>
    /// Disposes the rule set and suppresses finalization.
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
}

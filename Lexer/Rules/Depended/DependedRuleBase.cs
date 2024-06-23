#nullable disable
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules.Depended;
public abstract class DependedRuleBase : RuleBase, IDependedRule
{
    private readonly Dictionary<IRule, string[]> _dependencies = new();

    public ImmutableDictionary<IRule, string[]> Dependencies => _dependencies.ToImmutableDictionary();

    protected DependedRuleBase(string type, IRuleSettings ruleSettings) : base(type, ruleSettings) { }

    public void AddDependency(IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (_dependencies.ContainsKey(rule))
            throw new ArgumentException("'Dependencies' already contains specified rule", nameof(rule));
        _dependencies.Add(rule, names);
    }

    public void SetDependencyNames(IRule rule, params string[] names)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(names, nameof(names));

        if (!_dependencies.ContainsKey(rule))
            throw new ArgumentException("'Dependencies' do not contain the specified rule", nameof(rule));
        _dependencies[rule] = names;
    }

    public void RemoveDependency(IRule rule) => _dependencies.Remove(rule);

    public void ClearDependencies() => _dependencies.Clear();

    public abstract IEnumerable<IRawLexeme> FindLexemes(IDependedRuleInput input);

    public override IEnumerable<IRawLexeme> FindLexemes(IRuleInput input)
    {
        if (input is IDependedRuleInput dependedInput)
        {
            return FindLexemes(dependedInput);
        }
        throw new ArgumentException("Invalid 'input' type", nameof(input));
    }
}

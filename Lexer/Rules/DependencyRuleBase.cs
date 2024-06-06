#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
using System.Collections.Immutable;

namespace Lexer.Rules;
public abstract class DependencyRuleBase : IDependencyRule
{
    private string _type;
    private readonly List<IRule<IRuleInput>> _dependencies = new();

    public string Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value));
    }
    public bool IsIgnored { get; set; }
    public bool IsEnabled { get; set; }

    public ImmutableList<IRule<IRuleInput>> Dependencies => _dependencies.ToImmutableList();

    protected DependencyRuleBase(string type, bool isIgnored = false, bool isEnabled = true)
    {
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    public void AddDependency(IRule<IRuleInput> rule)
    {
        if (_dependencies.Contains(rule)) return;
        _dependencies.Add(rule);
    }

    public void RemoveDependency(IRule<IRuleInput> rule) => _dependencies.Remove(rule);

    public void ClearDependencies() => _dependencies.Clear();

    public abstract Task<AnalyzedLayer> FindLexemes(IDependencyRuleInput input, CancellationToken ct);
}

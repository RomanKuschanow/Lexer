#nullable disable
using Lexer;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.Visitors;

namespace Lexer.Rules.Common;
public abstract class RuleBase : IRule
{
    private string _type;

    public string Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value));
    }
    public bool IsIgnored { get; set; }
    public bool IsOnlyForDependentRules { get; set; }
    public bool IsEnabled { get; set; }

    protected RuleBase(IRuleSettings ruleSettings)
    {
        ArgumentNullException.ThrowIfNull(ruleSettings);

        Type = ruleSettings.Type;
        IsIgnored = ruleSettings.IsIgnored;
        IsOnlyForDependentRules = ruleSettings.IsOnlyForDependentRules;
        IsEnabled = ruleSettings.IsEnabled;
    }

    public abstract Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct);

    public IRuleInput Accept(IVisitor visitor, VisitorInput visitorInput) => visitor.Rule(visitorInput);
}

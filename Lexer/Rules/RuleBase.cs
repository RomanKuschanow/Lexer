#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
using Lexer.Rules.Visitors;

namespace Lexer.Rules;
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

    protected RuleBase(string type, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true)
    {
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    public abstract Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct);

    public IRuleInput Accept(IVisitor visitor, VisitorInput visitorInput) => visitor.Rule(visitorInput);
}

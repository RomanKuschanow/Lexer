#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;

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
    public bool IsEnabled { get; set; }

    protected RuleBase(string type, bool isIgnored = false, bool isEnabled = true)
    {
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    public abstract Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct);
}

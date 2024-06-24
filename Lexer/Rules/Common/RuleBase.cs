#nullable disable
using Lexer.Attributes;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.Common;
[UseThisRawLayerCreator(typeof(RawLayerCreator))]
[UseThisRuleInputCreator(typeof(CommonRuleInputCreator))]
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

    protected RuleBase(string type, IRuleSettings ruleSettings)
    {
        ArgumentNullException.ThrowIfNull(ruleSettings);

        Type = type;
        IsIgnored = ruleSettings.IsIgnored;
        IsOnlyForDependentRules = ruleSettings.IsOnlyForDependentRules;
        IsEnabled = ruleSettings.IsEnabled;
    }

    public abstract IEnumerable<IRawLexeme> FindLexemes(IRuleInput input);
}

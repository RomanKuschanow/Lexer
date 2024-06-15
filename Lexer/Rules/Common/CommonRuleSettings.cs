using Lexer.Rules.Interfaces;

namespace Lexer.Rules.Common;
public class CommonRuleSettings : IRuleSettings
{
    public string Type { get; }
    public bool IsIgnored {get;}
    public bool IsOnlyForDependentRules {get;}
    public bool IsEnabled {get;}

    public CommonRuleSettings(string type, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true)
    {
        Type = type;
        IsIgnored = isIgnored;
        IsOnlyForDependentRules = isOnlyForDependentRules;
        IsEnabled = isEnabled;
    }
}

using Lexer.Rules.Interfaces;

namespace Lexer.Rules.Common;
public class CommonRuleSettings : IRuleSettings
{
    public bool IsIgnored { get; }
    public bool IsOnlyForProcessing { get; }
    public bool IsEnabled { get; }

    public CommonRuleSettings(bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true)
    {
        IsIgnored = isIgnored;
        IsOnlyForProcessing = isOnlyForDependentRules;
        IsEnabled = isEnabled;
    }
}

using Lexer.Rules.Common;

namespace Lexer.Rules.Depended;
public class DependedRegexRuleSettings : CommonRuleSettings
{
    public DependedRuleOptions RuleOptions { get; }

    public DependedRegexRuleSettings(DependedRuleOptions ruleOptions = DependedRuleOptions.None, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true) : base(isIgnored, isOnlyForDependentRules, isEnabled)
    {
        RuleOptions = ruleOptions;
    }
}

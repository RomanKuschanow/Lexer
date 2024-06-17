using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;

namespace Lexer.Rules.Depended;
public class DependedRegexRuleSettings : CommonRuleSettings
{
    public DependedRuleOptions RuleOptions { get; }

    public DependedRegexRuleSettings(string type, DependedRuleOptions ruleOptions = DependedRuleOptions.None, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true) : base(type, isIgnored, isOnlyForDependentRules, isEnabled)
    {
        RuleOptions = ruleOptions;
    }
}

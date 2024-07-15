using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class CommonRuleInput : IRuleInput
{
    public string Text { get; init; }

    public CommonRuleInput(string text) => Text = text;
}

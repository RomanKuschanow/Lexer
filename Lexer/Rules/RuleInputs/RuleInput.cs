namespace Lexer.Rules.RuleInputs;
public class RuleInput : IRuleInput
{
    public string Text { get; init; }

    public RuleInput(string text) => Text = text;
}

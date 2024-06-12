using Lexer.Rules.RuleInputs;

namespace Lexer.Rules.Visitors;
public class Visitor : IVisitor
{
    public IRuleInput Rule(VisitorInput visitorInput) => new RuleInput(visitorInput.Text);
    public IDependencyRuleInput DependencyRule(VisitorInput visitorInput) => new DependencyRuleInput(visitorInput.Text, visitorInput.Layers);
}

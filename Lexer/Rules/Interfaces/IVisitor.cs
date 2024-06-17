using Lexer.Rules.Visitors;

namespace Lexer.Rules.Interfaces;
public interface IVisitor
{
    IRuleInput Rule(VisitorInput visitorInput);
    IDependedRuleInput DependencyRule(VisitorInput visitorInput, IDependedRule rule);
}

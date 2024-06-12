using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs;

namespace Lexer.Rules.Visitors;
public interface IVisitor
{
    IRuleInput Rule(VisitorInput visitorInput); 
    IDependencyRuleInput DependencyRule(VisitorInput visitorInput); 
}

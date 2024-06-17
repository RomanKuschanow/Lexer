using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;

namespace Lexer.Rules.Visitors;
public class Visitor : IVisitor
{
    public IRuleInput Rule(VisitorInput visitorInput) => new RuleInput(visitorInput.Text);
    public IDependencyRuleInput DependencyRule(VisitorInput visitorInput, IDependencyRule rule)
    {
        Dictionary<IRule, AnalyzedLayer> dependencies = new();
        foreach (IRule dependencyRule in rule.Dependencies.Keys)
            dependencies.Add(dependencyRule, visitorInput.Layers[dependencyRule]);

        return new DependedRuleInput(visitorInput.Text, dependencies);
    }
}

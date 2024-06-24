using Lexer.Rules.Interfaces;

namespace Lexer.Exceptions;
public class NecessaryAttributeNotFoundException : Exception
{
    public Type RuleType { get; init; }
    public Type AttributeType { get; init; }

    public NecessaryAttributeNotFoundException(IRule rule, Type attribute) : base($"Missing necessary attribute '{attribute}' in rule '{rule.GetType()}'")
    {
        RuleType = rule.GetType();
        AttributeType = attribute;
    }
}

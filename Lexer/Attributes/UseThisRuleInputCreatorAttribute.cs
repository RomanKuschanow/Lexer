using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class UseThisRuleInputCreatorAttribute : Attribute
{
    public Type Type { get; }

    public UseThisRuleInputCreatorAttribute(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type != typeof(IRuleInputCreator) && type.GetInterface("IRuleInputCreator") is null)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRuleInputCreator)}'", nameof(type));

        Type = type;
    }
}

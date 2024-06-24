using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class UseThisRawLayerCreatorAttribute : Attribute
{
    public Type Type { get; }

    public UseThisRawLayerCreatorAttribute(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type != typeof(IRawLayerCreator) && type.GetInterface("IRawLayerCreator") is null)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}'", nameof(type));

        Type = type;
    }
}

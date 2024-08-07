﻿using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class UseThisRawLayerCreatorAttribute : Attribute
{
    public Type RawLayerCreatorType { get; }

    public UseThisRawLayerCreatorAttribute(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type != typeof(IRawLayerCreator) && type.GetInterface("IRawLayerCreator") is null)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}'", nameof(type));

        RawLayerCreatorType = type;
    }
}

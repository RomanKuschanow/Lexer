﻿#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerFactory : IRawLayerFactory
{
    /// <summary>
    /// Dictionary to store raw layer creators by their type.
    /// </summary>
    private Dictionary<Type, IRawLayerCreator> _rawLayerCreators = new();

    /// <summary>
    /// Gets the collection of raw layer creators.
    /// </summary>
    public IEnumerable<IRawLayerCreator> RawLayerCreators => _rawLayerCreators.Values;

    /// <summary>
    /// Adds a concrete raw layer creator to the factory.
    /// </summary>
    /// <param name="rawLayerCreator">The raw layer creator to add.</param>
    /// <returns><c>true</c> if the creator was added successfully; otherwise, <c>false</c>.</returns>
    public bool AddConcreteCreator(IRawLayerCreator rawLayerCreator) => _rawLayerCreators.TryAdd(rawLayerCreator.GetType(), rawLayerCreator);

    /// <summary>
    /// Creates a raw layer using the specified creator type, raw lexemes, and rule.
    /// </summary>
    /// <param name="creatorType">The type of the raw layer creator.</param>
    /// <param name="rawLexemes">The collection of raw lexemes.</param>
    /// <param name="rule">The rule associated with the raw layer.</param>
    /// <returns>The created raw layer.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="creatorType"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="creatorType"/> is not a class or does not implement <see cref="IRawLayerCreator"/>.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a creator of the specified type is not found.</exception>
    public IRawLayer CreateRawLayer(Type creatorType, IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(creatorType);

        if (creatorType.GetInterface(nameof(IRawLayerCreator)) is null || !creatorType.IsClass)
            throw new ArgumentException($"'{nameof(creatorType)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}' and must be a class", nameof(creatorType));

        if (!_rawLayerCreators.TryGetValue(creatorType, out IRawLayerCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(rawLexemes, rule);
    }

    /// <summary>
    /// Disposes the factory and suppresses finalization.
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
}

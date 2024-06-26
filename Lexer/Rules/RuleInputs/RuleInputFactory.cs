#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class RuleInputFactory : IRuleInputFactory
{
    /// <summary>
    /// Dictionary to store rule input creators by their type.
    /// </summary>
    private Dictionary<Type, IRuleInputCreator> _inputCreators = new();

    /// <summary>
    /// Gets the collection of rule input creators.
    /// </summary>
    public IEnumerable<IRuleInputCreator> RuleInputCreators => _inputCreators.Values;

    /// <summary>
    /// Adds a concrete rule input creator to the factory.
    /// </summary>
    /// <param name="inputCreator">The rule input creator to add.</param>
    /// <returns><c>true</c> if the creator was added successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="inputCreator"/> is null.</exception>
    public bool AddConcreteCreator(IRuleInputCreator inputCreator)
    {
        ArgumentNullException.ThrowIfNull(inputCreator);

        return _inputCreators.TryAdd(inputCreator.GetType(), inputCreator);
    }

    /// <summary>
    /// Creates a rule input using the specified creator type and intermediate data collection.
    /// </summary>
    /// <param name="creatorType">The type of the rule input creator.</param>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="creatorType"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="creatorType"/> is not a class or does not implement <see cref="IRuleInputCreator"/>.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a creator of the specified type is not found.</exception>
    public IRuleInput CreateInput(Type creatorType, IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(creatorType);

        if (creatorType.GetInterface(nameof(IRuleInputCreator)) is null || !creatorType.IsClass)
            throw new ArgumentException($"'{nameof(creatorType)}' must be in the inheritance hierarchy of '{typeof(IRuleInputCreator)}' and must be a class", nameof(creatorType));

        if (!_inputCreators.TryGetValue(creatorType, out IRuleInputCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(dataCollection);
    }

    /// <summary>
    /// Disposes the factory and suppresses finalization.
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
}

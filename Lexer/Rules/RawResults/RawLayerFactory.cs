#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerFactory : IRawLayerFactory
{
    private Dictionary<Type, IRawLayerCreator> _rawLayerCreators = [];

    public IEnumerable<IRawLayerCreator> RawLayerCreators => _rawLayerCreators.Values;

    public bool AddConcreteCreator(IRawLayerCreator rawLayerCreator) => _rawLayerCreators.TryAdd(rawLayerCreator.GetType(), rawLayerCreator);

    public IRawLayer CreateRawLayer(Type creatorType, IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(creatorType);

        if (creatorType.GetInterface("IRawLayerCreator") is null || !creatorType.IsClass)
            throw new ArgumentException($"'{nameof(creatorType)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}' and must be a class", nameof(creatorType));

        if (_rawLayerCreators.TryGetValue(creatorType, out IRawLayerCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(rawLexemes, rule);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}

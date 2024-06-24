#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerFactory
{
    private Dictionary<Type, IRawLayerCreator> _rawLayerCreators = [];

    public IEnumerable<IRawLayerCreator> RawLayerCreators => _rawLayerCreators.Values;

    public bool AddConcreteCreator(IRawLayerCreator rawLayerCreator) => _rawLayerCreators.TryAdd(rawLayerCreator.GetType(), rawLayerCreator);

    public IRawLayer CreateRawLayer(Type type, IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.GetInterface("IRawLayerCreator") is null || !type.IsClass)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}' and must be a class", nameof(type));

        if (_rawLayerCreators.TryGetValue(type, out IRawLayerCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(rawLexemes, rule);
    }
}

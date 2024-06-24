#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerFactory
{
    private Dictionary<Type, IRawLayerCreator> RawLayerCreators = [];

    public bool AddConcreteCreator(IRawLayerCreator rawLayerCreator) => RawLayerCreators.TryAdd(rawLayerCreator.GetType(), rawLayerCreator);

    public IRawLayer CreateRawLayer(Type type, IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.GetInterface("IRawLayerCreator") is null || !type.IsClass)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRawLayerCreator)}' and must be a class", nameof(type));

        if (RawLayerCreators.TryGetValue(type, out IRawLayerCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(rawLexemes, rule);
    }
}

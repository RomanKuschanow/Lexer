#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerFactory
{
    private Dictionary<Type, IRawLayerCreator> RawLayerCreators = new();

    public bool AddConcreteCreator<T>(IRawLayerCreator rawLayerCreator) where T : IRule => RawLayerCreators.TryAdd(typeof(T), rawLayerCreator);

    public IRawLayer CreateRawLayer(IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        if (RawLayerCreators.TryGetValue(rule.GetType(), out IRawLayerCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(rawLexemes, rule);
    }
}

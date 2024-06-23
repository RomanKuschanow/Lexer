#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class RuleInputFactory
{
    private Dictionary<Type, IRuleInputCreator> InputCreators = new();

    public bool AddConcreteCreator<T>(IRuleInputCreator inputCreator) where T : IRule => InputCreators.TryAdd(typeof(T), inputCreator);

    public IRuleInput CreateInput(Type type, IntermediateDataCollection dataCollection)
    {
        if (InputCreators.TryGetValue(type, out IRuleInputCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(dataCollection);
    }
}

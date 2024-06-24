#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class RuleInputFactory
{
    private Dictionary<Type, IRuleInputCreator> InputCreators = [];

    public bool AddConcreteCreator(IRuleInputCreator inputCreator) => InputCreators.TryAdd(inputCreator.GetType(), inputCreator);

    public IRuleInput CreateInput(Type type, IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.GetInterface("IRuleInputCreator") is null || !type.IsClass)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IRuleInputCreator)}' and must be a class", nameof(type));

        if (InputCreators.TryGetValue(type, out IRuleInputCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(dataCollection);
    }
}

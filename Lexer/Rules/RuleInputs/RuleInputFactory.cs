#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class RuleInputFactory : IRuleInputFactory
{
    private Dictionary<Type, IRuleInputCreator> _inputCreators = [];

    public IEnumerable<IRuleInputCreator> RuleInputCreators => _inputCreators.Values;

    public bool AddConcreteCreator(IRuleInputCreator inputCreator)
    {
        ArgumentNullException.ThrowIfNull(inputCreator);

        return _inputCreators.TryAdd(inputCreator.GetType(), inputCreator);
    }

    public IRuleInput CreateInput(Type creatorType, IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(creatorType);

        if (creatorType.GetInterface("IRuleInputCreator") is null || !creatorType.IsClass)
            throw new ArgumentException($"'{nameof(creatorType)}' must be in the inheritance hierarchy of '{typeof(IRuleInputCreator)}' and must be a class", nameof(creatorType));

        if (_inputCreators.TryGetValue(creatorType, out IRuleInputCreator creator))
            throw new KeyNotFoundException();

        return creator.Create(dataCollection);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}

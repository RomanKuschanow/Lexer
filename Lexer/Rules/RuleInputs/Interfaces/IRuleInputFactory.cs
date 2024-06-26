using Lexer.Analyzer.IntermediateData;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IRuleInputFactory : IDisposable
{
    IEnumerable<IRuleInputCreator> RuleInputCreators { get; }

    IRuleInput CreateInput(Type creatorType, IntermediateDataCollection dataCollection);
}

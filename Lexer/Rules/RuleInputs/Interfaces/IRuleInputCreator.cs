using Lexer.Analyzer.IntermediateData;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IRuleInputCreator
{
    IRuleInput Create(IntermediateDataCollection dataCollection);
}

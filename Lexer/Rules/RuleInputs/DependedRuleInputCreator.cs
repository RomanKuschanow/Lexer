using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class DependedRuleInputCreator : IRuleInputCreator
{
    public IRuleInput Create(IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(dataCollection);

        string text = dataCollection.Get<InputTextIntermediateData>().GetData();
        var layers = dataCollection.Get<ProcessedLayersIntermediateData>().GetData();

        return new DependedRuleInput(text, layers);
    }
}

using Lexer.Analyzer.Interfaces;
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class DependedRuleInputCreator : IRuleInputCreator
{
    /// <summary>
    /// Creates a depended rule input using the specified intermediate data collection.
    /// </summary>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataCollection"/> is null.</exception>
    public IRuleInput Create(IIntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(dataCollection);

        string text = dataCollection.Get<InputTextIntermediateData>().GetData();
        var layers = dataCollection.Get<ProcessedLayersIntermediateData>().GetData();

        return new DependedRuleInput(text, layers);
    }
}

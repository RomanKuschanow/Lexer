using Lexer.Analyzer.IntermediateData;
using Lexer.Attributes;
using Lexer.Rules.Common;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;

public class CommonRuleInputCreator : IRuleInputCreator
{
    /// <summary>
    /// Creates a rule input using the specified intermediate data collection.
    /// </summary>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataCollection"/> is null.</exception>
    public IRuleInput Create(IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(dataCollection);

        string text = dataCollection.Get<InputTextIntermediateData>().GetData();

        return new CommonRuleInput(text);
    }
}

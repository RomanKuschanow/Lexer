using Lexer.Analyzer.IntermediateData.Interfaces;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IRuleInputCreator
{
    /// <summary>
    /// Creates a rule input using the specified intermediate data collection.
    /// </summary>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    IRuleInput Create(IIntermediateDataCollection dataCollection);
}

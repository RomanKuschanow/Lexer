using Lexer.Analyzer.IntermediateData;

namespace Lexer.Rules.RuleInputs.Interfaces;
public interface IRuleInputFactory : IDisposable
{
    /// <summary>
    /// Gets the collection of rule input creators.
    /// </summary>
    IEnumerable<IRuleInputCreator> RuleInputCreators { get; }

    /// <summary>
    /// Creates a rule input using the specified creator type and intermediate data collection.
    /// </summary>
    /// <param name="creatorType">The type of the rule input creator.</param>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    IRuleInput CreateInput(Type creatorType, IntermediateDataCollection dataCollection);
}

using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Rules.Interfaces;

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
    /// <param name="rule">The target rule.</param>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    /// <returns>The created rule input.</returns>
    IRuleInput CreateInput(IRule rule, IIntermediateDataCollection dataCollection);
}

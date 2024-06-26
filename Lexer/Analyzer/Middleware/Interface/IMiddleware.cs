using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Analyzer.Middleware.Interface;
public interface IMiddleware
{
    /// <summary>
    /// Executes the middleware with the specified rule, rule input, raw layer, and intermediate data collection.
    /// </summary>
    /// <param name="rule">Connected rule.</param>
    /// <param name="ruleInput">The input for the rule.</param>
    /// <param name="rawLayer">The raw layer from <paramref name="rule"/>.</param>
    /// <param name="dataCollection">The collection of intermediate data.</param>
    void Execute(IRule rule, IRuleInput ruleInput, IRawLayer rawLayer, IntermediateDataCollection dataCollection);
}

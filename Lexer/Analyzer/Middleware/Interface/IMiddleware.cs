using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Analyzer.Middleware.Interface;
public interface IMiddleware
{
    void Execute(IRule rule, IRuleInput ruleInput, IRawLayer rawLayer, IntermediateDataCollection dataCollection);
}

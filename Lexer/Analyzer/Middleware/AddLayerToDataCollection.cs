using Lexer.Analyzer.Interfaces;
using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Analyzer.Middleware;
public class AddLayerToDataCollection : IMiddleware
{
    public void Execute(IRule rule, IRuleInput ruleInput, IRawLayer rawLayer, IIntermediateDataCollection dataCollection)
    {
        dataCollection.TryAdd(new ProcessedLayersIntermediateData());

        var processedLayers = dataCollection.Get<ProcessedLayersIntermediateData>();

        processedLayers.SetDataByKey(rule, rawLayer);
    }
}

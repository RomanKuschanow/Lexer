#nullable disable
using Lexer.Analyzer.Interfaces;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class ProcessedLayersIntermediateData : IDictionaryIntermediateData<IRule, IRawLayer>
{
    private Dictionary<IRule, IRawLayer> ProcessedLayers { get; set; }

    public IRawLayer GetDataByKey(IRule key)
    {
        ArgumentNullException.ThrowIfNull(ProcessedLayers);
        ArgumentNullException.ThrowIfNull(key);

        return ProcessedLayers[key];
    }

    public void SetDataByKey(IRule key, IRawLayer value)
    {
        ArgumentNullException.ThrowIfNull(ProcessedLayers);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        ProcessedLayers.Add(key, value);
    }

    public IDictionary<IRule, IRawLayer> GetData() => ProcessedLayers;

    public void SetData(IDictionary<IRule, IRawLayer> data) => ProcessedLayers = data.ToDictionary();
}

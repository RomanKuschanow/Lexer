#nullable disable
using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class ProcessedLayersIntermediateData : IDictionaryIntermediateData<IRule, IRawLayer>
{
    /// <summary>
    /// Gets or sets the dictionary of processed layers.
    /// </summary>
    private Dictionary<IRule, IRawLayer> ProcessedLayers { get; set; }

    public ProcessedLayersIntermediateData() : this(new Dictionary<IRule, IRawLayer>()) { }

    public ProcessedLayersIntermediateData(IDictionary<IRule, IRawLayer> processedLayers)
    {
        ProcessedLayers = (processedLayers ?? throw new ArgumentNullException(nameof(processedLayers))).ToDictionary();
    }

    /// <summary>
    /// Retrieves the raw layer associated with the specified rule.
    /// </summary>
    /// <param name="key">The rule whose raw layer is to be retrieved.</param>
    /// <returns>The raw layer associated with the specified rule.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> or <see cref="ProcessedLayers"/> is null.</exception>
    public IRawLayer GetDataByKey(IRule key)
    {
        ArgumentNullException.ThrowIfNull(ProcessedLayers);
        ArgumentNullException.ThrowIfNull(key);

        return ProcessedLayers[key];
    }

    /// <summary>
    /// Sets the raw layer for the specified rule.
    /// </summary>
    /// <param name="key">The rule whose raw layer is to be set.</param>
    /// <param name="value">The raw layer to set for the specified rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/>, <paramref name="value"/>, or <see cref="ProcessedLayers"/> is null.</exception>
    public void SetDataByKey(IRule key, IRawLayer value)
    {
        ArgumentNullException.ThrowIfNull(ProcessedLayers);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        ProcessedLayers.Add(key, value);
    }

    /// <summary>
    /// Retrieves the dictionary of processed layers.
    /// </summary>
    /// <returns>The dictionary of processed layers.</returns>
    public IDictionary<IRule, IRawLayer> GetData() => ProcessedLayers;

    /// <summary>
    /// Sets the dictionary of processed layers.
    /// </summary>
    /// <param name="data">The dictionary of processed layers to set.</param>
    public void SetData(IDictionary<IRule, IRawLayer> data) => ProcessedLayers = data.ToDictionary();
    object IIntermediateData.GetData() => ProcessedLayers;
    void IIntermediateData.SetData(object data) => ProcessedLayers = (Dictionary<IRule, IRawLayer>)data;
}

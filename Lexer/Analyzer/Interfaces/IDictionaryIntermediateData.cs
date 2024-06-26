namespace Lexer.Analyzer.Interfaces;
public interface IDictionaryIntermediateData<TKey, TValue> : IIntermediateData<IDictionary<TKey, TValue>>
{
    /// <summary>
    /// Retrieves the data associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose data is to be retrieved.</param>
    /// <returns>The data associated with the specified key.</returns>
    TValue GetDataByKey(TKey key);

    /// <summary>
    /// Sets the data for the specified key.
    /// </summary>
    /// <param name="key">The key whose data is to be set.</param>
    /// <param name="value">The data to be set for the specified key.</param>
    void SetDataByKey(TKey key, TValue value);
}

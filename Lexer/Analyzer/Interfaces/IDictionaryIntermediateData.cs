namespace Lexer.Analyzer.Interfaces;
public interface IDictionaryIntermediateData<TKey, TValue> : IIntermediateData<IDictionary<TKey, TValue>>
{
    TValue GetDataByKey(TKey key);
    void SetDataByKey(TKey key, TValue value);
}

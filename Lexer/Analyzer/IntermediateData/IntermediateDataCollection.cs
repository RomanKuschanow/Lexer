#nullable disable
using Lexer.Analyzer.Interfaces;
using System.Reflection;

namespace Lexer.Analyzer.IntermediateData;
public class IntermediateDataCollection
{
    private Dictionary<Type, IIntermediateData<object>> _dataDict = new();

    public void Add<T>(T data)
    {
        if (data is not IIntermediateData<object> convertedData)
            throw new InvalidCastException($"Cannot convert '{data.GetType()}' to '{typeof(IIntermediateData<object>)}'");

        _dataDict.Add(data.GetType(), convertedData);
    }

    public void TryAdd<T>(T data)
    {
        if (data is not IIntermediateData<object> convertedData)
            throw new InvalidCastException($"Cannot convert '{data.GetType()}' to '{typeof(IIntermediateData<object>)}'");

        _dataDict.TryAdd(data.GetType(), convertedData);
    }

    public T Get<T>() => (T)_dataDict[typeof(T)];

    public void Remove<T>() => _dataDict.Remove(typeof(T));
}

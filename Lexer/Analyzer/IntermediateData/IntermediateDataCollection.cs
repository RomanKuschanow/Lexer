#nullable disable
using Lexer.Analyzer.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class IntermediateDataCollection
{
    private Dictionary<Type, IIntermediateData<object>> _dataDict = new();

    public void Add<T>(T data) where T : IIntermediateData<object> => _dataDict.Add(data.GetType(), data);

    public void TryAdd<T>(T data) where T : IIntermediateData<object> => _dataDict.TryAdd(data.GetType(), data);

    public T Get<T>() where T : IIntermediateData<object> => (T)_dataDict[typeof(T)];

    public void Remove<T>() where T : IIntermediateData<object> => _dataDict.Remove(typeof(T));
}

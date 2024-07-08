namespace Lexer.Analyzer.IntermediateData.Interfaces;
public interface IIntermediateDataCollection
{
    public void Add<T>(T data) where T : IIntermediateData;
    public void TryAdd<T>(T data) where T : IIntermediateData;
    public T Get<T>() where T : IIntermediateData;
    public void Remove<T>() where T : IIntermediateData;
}

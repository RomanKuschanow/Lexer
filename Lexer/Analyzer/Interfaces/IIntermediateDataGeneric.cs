namespace Lexer.Analyzer.Interfaces;
public interface IIntermediateData<T> : IIntermediateData
{
    new T GetData();

    void SetData(T data);
}

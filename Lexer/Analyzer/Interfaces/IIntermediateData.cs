namespace Lexer.Analyzer.Interfaces;
public interface IIntermediateData<T>
{
    T GetData();

    void SetData(T data);
}

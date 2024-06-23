namespace Lexer.Analyzer.Interfaces;
public interface IIntermediateData<out T>
{
    T GetData();

    void SetData(T data);
}

namespace Lexer.Analyzer.Interfaces;
public interface IIntermediateData<T>
{
    /// <summary>
    /// Retrieves the intermediate data.
    /// </summary>
    /// <returns>The intermediate data.</returns>
    T GetData();

    /// <summary>
    /// Sets the intermediate data.
    /// </summary>
    /// <param name="data">The data to set.</param>
    void SetData(T data);
}

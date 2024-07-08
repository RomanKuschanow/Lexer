namespace Lexer.Analyzer.IntermediateData.Interfaces;
public interface IIntermediateData
{
    /// <summary>
    /// Retrieves the intermediate data.
    /// </summary>
    /// <returns>The intermediate data.</returns>
    object GetData();

    /// <summary>
    /// Sets the intermediate data.
    /// </summary>
    /// <param name="data">The data to set.</param>
    void SetData(object data);
}

#nullable disable
using Lexer.Analyzer.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class IntermediateDataCollection : IIntermediateDataCollection
{
    /// <summary>
    /// Dictionary to store intermediate data by their type.
    /// </summary>
    private Dictionary<Type, IIntermediateData> _dataDict = new();

    /// <summary>
    /// Adds intermediate data to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="data">The data to add.</param>
    /// <exception cref="InvalidCastException">Thrown when the data cannot be converted to <see cref="IIntermediateData"/>.</exception>
    public void Add<T>(T data) where T : IIntermediateData
    {
        _dataDict.Add(data.GetType(), data);
    }

    /// <summary>
    /// Tries to add intermediate data to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="data">The data to try to add.</param>
    /// <exception cref="InvalidCastException">Thrown when the data cannot be converted to <see cref="IIntermediateData"/>.</exception>
    public void TryAdd<T>(T data) where T : IIntermediateData
    {
        _dataDict.TryAdd(data.GetType(), data);
    }

    /// <summary>
    /// Retrieves intermediate data of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data to retrieve.</typeparam>
    /// <returns>The data of the specified type.</returns>
    public T Get<T>() where T : IIntermediateData => (T)_dataDict[typeof(T)];

    /// <summary>
    /// Removes intermediate data of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data to remove.</typeparam>
    public void Remove<T>() where T : IIntermediateData => _dataDict.Remove(typeof(T));
}

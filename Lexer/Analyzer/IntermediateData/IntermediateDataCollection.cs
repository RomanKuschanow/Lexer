#nullable disable
using Lexer.Analyzer.Interfaces;
using System.Reflection;

namespace Lexer.Analyzer.IntermediateData;
public class IntermediateDataCollection
{
    /// <summary>
    /// Dictionary to store intermediate data by their type.
    /// </summary>
    private Dictionary<Type, IIntermediateData<object>> _dataDict = new();

    /// <summary>
    /// Adds intermediate data to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="data">The data to add.</param>
    /// <exception cref="InvalidCastException">Thrown when the data cannot be converted to <see cref="IIntermediateData{T}"/>.</exception>
    public void Add<T>(T data)
    {
        if (data is not IIntermediateData<object> convertedData)
            throw new InvalidCastException($"Cannot convert '{data.GetType()}' to '{typeof(IIntermediateData<object>)}'");

        _dataDict.Add(data.GetType(), convertedData);
    }

    /// <summary>
    /// Tries to add intermediate data to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="data">The data to try to add.</param>
    /// <exception cref="InvalidCastException">Thrown when the data cannot be converted to <see cref="IIntermediateData{T}"/>.</exception>
    public void TryAdd<T>(T data)
    {
        if (data is not IIntermediateData<object> convertedData)
            throw new InvalidCastException($"Cannot convert '{data.GetType()}' to '{typeof(IIntermediateData<object>)}'");

        _dataDict.TryAdd(data.GetType(), convertedData);
    }

    /// <summary>
    /// Retrieves intermediate data of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data to retrieve.</typeparam>
    /// <returns>The data of the specified type.</returns>
    public T Get<T>() => (T)_dataDict[typeof(T)];

    /// <summary>
    /// Removes intermediate data of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the data to remove.</typeparam>
    public void Remove<T>() => _dataDict.Remove(typeof(T));
}

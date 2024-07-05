#nullable disable
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Attributes;
using Lexer.Exceptions;
using Lexer.Rules.Interfaces;

namespace Lexer.Analyzer.Middleware;
public class MiddlewareCollection : IMiddlewareCollection
{
    /// <summary>
    /// Dictionary to store middleware by their type.
    /// </summary>
    private Dictionary<Type, IMiddleware> _middleware = new();

    public IEnumerable<IMiddleware> Middleware => _middleware.Values;

    /// <summary>
    /// Adds middleware to the collection.
    /// </summary>
    /// <param name="data">The middleware to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    public void Add(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _middleware.Add(data.GetType(), data);
    }

    /// <summary>
    /// Tries to add middleware to the collection.
    /// </summary>
    /// <param name="data">The middleware to try to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    public bool TryAdd(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return _middleware.TryAdd(data.GetType(), data);
    }

    /// <summary>
    /// Retrieves middleware by its type.
    /// </summary>
    /// <param name="middlewareType">The type of the middleware to retrieve.</param>
    /// <returns>The middleware of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="middlewareType"/> is null.</exception>
    public IMiddleware Get(Type middlewareType)
    {
        ArgumentNullException.ThrowIfNull(middlewareType);

        _middleware.TryGetValue(middlewareType, out IMiddleware middleware);

        return middleware;
    }

    /// <summary>
    /// Removes middleware of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the middleware to remove.</typeparam>
    /// <returns>True if the middleware was successfully removed; otherwise, false.</returns>
    public bool Remove(Type middlewareType) => _middleware.Remove(middlewareType);

    /// <summary>
    /// Retrieves middleware associated with the specified rule.
    /// </summary>
    /// <param name="rule">The rule to get the associated middleware for.</param>
    /// <returns>A collection of middleware associated with the specified rule.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="rule"/> is null.</exception>
    /// <exception cref="NecessaryMiddlewareNotFoundException">Thrown when a necessary middleware is not found.</exception>
    public IEnumerable<IMiddleware> GetMiddlewareByRule(IRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        foreach (UseThisMiddlewareAttribute atr in rule.GetType().GetCustomAttributes(typeof(UseThisMiddlewareAttribute), true))
        {
            IMiddleware middleware;
            if ((middleware = Get(atr.MiddlewareType)) is null)
            {
                if (atr.Necessary)
                    throw new NecessaryMiddlewareNotFoundException(rule, atr.MiddlewareType);
            }

            yield return middleware;
        }
    }

    /// <summary>
    /// Disposes the middleware collection.
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
}
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
    private Dictionary<Type, IEnumerable<IMiddleware>> _middleware = new();

    public IDictionary<Type, IEnumerable<IMiddleware>> Middleware => _middleware;

    /// <summary>
    /// Adds middleware to the collection.
    /// </summary>
    /// <param name="ruleType">The binding rule type.</param>
    /// <param name="middleware">The middleware to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="middleware"/> is null.</exception>
    public void Add(Type ruleType, params IMiddleware[] middleware)
    {
        ArgumentNullException.ThrowIfNull(ruleType);
        ArgumentNullException.ThrowIfNull(middleware);

        if (_middleware.ContainsKey(ruleType))
            _middleware.Add(middleware.GetType(), _middleware[ruleType].Concat(middleware));
        else
            _middleware.Add(ruleType, middleware);
    }

    /// <summary>
    /// Adds middleware to the collection.
    /// </summary>
    /// <typeparam name="T">The rule type to removing.</typeparam>
    /// <param name="middleware">The middleware to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="middleware"/> is null.</exception>
    public void Add<T>(params IMiddleware[] middleware) where T : IRule
    {
        ArgumentNullException.ThrowIfNull(middleware);

        if (_middleware.ContainsKey(typeof(T)))
            _middleware.Add(middleware.GetType(), _middleware[typeof(T)].Concat(middleware));
        else
            _middleware.Add(typeof(T), middleware);
    }

    /// <summary>
    /// Tries to add middleware to the collection.
    /// </summary>
    /// <param name="ruleType">The binding rule type.</param>
    /// <param name="middleware">The middleware to try to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="middleware"/> is null.</exception>
    public bool TryAdd(Type ruleType, params IMiddleware[] middleware)
    {
        ArgumentNullException.ThrowIfNull(ruleType);
        ArgumentNullException.ThrowIfNull(middleware);

        if (_middleware.ContainsKey(ruleType))
            return _middleware.TryAdd(middleware.GetType(), _middleware[ruleType].Concat(middleware));
        else
            return _middleware.TryAdd(ruleType, middleware);
    }

    /// <summary>
    /// Tries to add middleware to the collection.
    /// </summary>
    /// <typeparam name="T">The rule type to removing.</typeparam>
    /// <param name="middleware">The middleware to try to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="middleware"/> is null.</exception>
    public bool TryAdd<T>(params IMiddleware[] middleware) where T : IRule
    {
        ArgumentNullException.ThrowIfNull(middleware);

        if (_middleware.ContainsKey(typeof(T)))
            return _middleware.TryAdd(middleware.GetType(), _middleware[typeof(T)].Concat(middleware));
        else
            return _middleware.TryAdd(typeof(T), middleware);
    }

    /// <summary>
    /// Removes middleware of the specified type from the collection.
    /// </summary>
    /// <param name="ruleType">The rule type to removing.</param>
    /// <returns>True if the middleware was successfully removed; otherwise, false.</returns>
    public bool Remove(Type ruleType) => _middleware.Remove(ruleType);

    /// <summary>
    /// Removes middleware of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The rule type to removing.</typeparam>
    /// <returns>True if the middleware was successfully removed; otherwise, false.</returns>
    public bool Remove<T>() where T : IRule => _middleware.Remove(typeof(T));

    /// <summary>
    /// Removes middleware of the specified type from the collection.
    /// </summary>
    /// <param name="ruleType">The rule type to removing.</param>
    /// <param name="middleware">The middleware to removing.</param>
    /// <returns>True if the middleware was successfully removed; otherwise, false.</returns>
    public void Remove(Type ruleType, params IMiddleware[] middleware) => _middleware[ruleType] = _middleware[ruleType].Except(middleware);

    /// <summary>
    /// Removes middleware of the specified type from the collection.
    /// </summary>
    /// <typeparam name="T">The rule type to removing.</typeparam>
    /// <param name="middleware">The middleware to removing.</param>
    /// <returns>True if the middleware was successfully removed; otherwise, false.</returns>
    public void Remove<T>(params IMiddleware[] middleware) where T : IRule => _middleware[typeof(T)] = _middleware[typeof(T)].Except(middleware);

    /// <summary>
    /// Retrieves middleware associated with the specified rule.
    /// </summary>
    /// <param name="rule">The rule to get the associated middleware for.</param>
    /// <returns>A collection of middleware associated with the specified rule.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="rule"/> is null.</exception>
    /// <exception cref="NecessaryMiddlewareNotFoundException">Thrown when a necessary middleware is not found.</exception>
    public IEnumerable<IMiddleware> Get(IRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        _middleware.TryGetValue(rule.GetType(), out IEnumerable<IMiddleware> result);

        return result ?? throw new KeyNotFoundException();
    }

    /// <summary>
    /// Disposes the middleware collection.
    /// </summary>
    public void Dispose() => GC.SuppressFinalize(this);
}
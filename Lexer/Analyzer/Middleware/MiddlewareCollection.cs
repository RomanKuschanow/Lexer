#nullable disable
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Attributes;
using Lexer.Exceptions;
using Lexer.Rules.Interfaces;
using System.Data;

namespace Lexer.Analyzer.Middleware;
public class MiddlewareCollection : IMiddlewareCollection
{
    private Dictionary<Type, IMiddleware> _middleware = [];

    public IEnumerable<IMiddleware> Middleware => _middleware.Values;

    public void Add(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _middleware.Add(data.GetType(), data);
    }

    public void TryAdd(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _middleware.TryAdd(data.GetType(), data);
    }

    public IMiddleware Get(Type middlewareType)
    {
        ArgumentNullException.ThrowIfNull(middlewareType);

        _middleware.TryGetValue(middlewareType, out IMiddleware middleware);

        return middleware;
    }

    public bool Remove<T>() where T : IRule => _middleware.Remove(typeof(T));

    public IEnumerable<IMiddleware> GetMiddlewareByRule(IRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        foreach (UseThisMiddlewareAttribute atr in rule.GetType().GetCustomAttributes(typeof(UseThisMiddlewareAttribute), true))
        {
            IMiddleware middleware;
            if ((middleware = Get(atr.MiddlewareType)) is null)
            {
                if (atr.Necessary)
                    throw new NecessaryMiddlewareNotFoundException(rule, middleware);
            }

            yield return middleware;
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}

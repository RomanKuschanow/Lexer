#nullable disable
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Attributes;
using Lexer.Exceptions;
using Lexer.Rules.Interfaces;
using System.Data;

namespace Lexer.Analyzer.Middleware;
public class MiddlewareCollection
{
    private Dictionary<Type, IMiddleware> _middlewareDict = [];

    public void Add(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _middlewareDict.Add(data.GetType(), data);
    }

    public void TryAdd(IMiddleware data)
    {
        ArgumentNullException.ThrowIfNull(data);

        _middlewareDict.TryAdd(data.GetType(), data);
    }

    public IMiddleware Get(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        _middlewareDict.TryGetValue(type, out IMiddleware middleware);

        return middleware;
    }

    public bool Remove<T>() where T : IRule => _middlewareDict.Remove(typeof(T));

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
}

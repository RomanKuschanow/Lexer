using Lexer.Analyzer.Middleware.Interface;
using Lexer.Rules.Interfaces;

namespace Lexer.Analyzer.Middleware;
public class MiddlewareCollection
{
    private Dictionary<Type, IMiddleware> _dataDict = new();

    public void Add<T>(IMiddleware data) where T : IRule => _dataDict.Add(typeof(T), data);

    public void TryAdd<T>(IMiddleware data) where T : IRule => _dataDict.TryAdd(typeof(T), data);

    public IMiddleware? Get(Type type)
    {
        _dataDict.TryGetValue(type, out var middleware);

        return middleware;
    }

    public bool Remove<T>() where T : IRule => _dataDict.Remove(typeof(T));
}

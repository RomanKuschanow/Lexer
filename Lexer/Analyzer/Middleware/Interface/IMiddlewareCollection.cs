using Lexer.Rules.Interfaces;

namespace Lexer.Analyzer.Middleware.Interface;
public interface IMiddlewareCollection : IDisposable
{
    IEnumerable<IMiddleware> Middleware { get; }

    IMiddleware Get(Type middlewareType);

    IEnumerable<IMiddleware> GetMiddlewareByRule(IRule rule);
}

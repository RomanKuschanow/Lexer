using Lexer.Rules.Interfaces;

namespace Lexer.Analyzer.Middleware.Interface;
public interface IMiddlewareCollection : IDisposable
{
    /// <summary>
    /// Gets the collection of middleware.
    /// </summary>
    IDictionary<Type, IEnumerable<IMiddleware>> Middleware { get; }

    /// <summary>
    /// Retrieves middleware associated with the specified rule.
    /// </summary>
    /// <param name="rule">The rule to get the associated middleware for.</param>
    /// <returns>A collection of middleware associated with the specified rule.</returns>
    IEnumerable<IMiddleware> Get(IRule rule);
}

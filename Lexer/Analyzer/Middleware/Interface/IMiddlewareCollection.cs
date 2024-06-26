using Lexer.Rules.Interfaces;

namespace Lexer.Analyzer.Middleware.Interface;
public interface IMiddlewareCollection : IDisposable
{
    /// <summary>
    /// Gets the collection of middleware.
    /// </summary>
    IEnumerable<IMiddleware> Middleware { get; }

    /// <summary>
    /// Retrieves a specific middleware by its type.
    /// </summary>
    /// <param name="middlewareType">The type of the middleware to retrieve.</param>
    /// <returns>The middleware of the specified type.</returns>
    IMiddleware Get(Type middlewareType);

    /// <summary>
    /// Retrieves middleware associated with the specified rule.
    /// </summary>
    /// <param name="rule">The rule to get the associated middleware for.</param>
    /// <returns>A collection of middleware associated with the specified rule.</returns>
    IEnumerable<IMiddleware> GetMiddlewareByRule(IRule rule);
}

using Lexer.Analyzer.Middleware.Interface;
using Lexer.Rules.Interfaces;

namespace Lexer.Exceptions;
public class NecessaryMiddlewareNotFoundException : Exception
{
    public Type RuleType { get; init; }
    public Type MiddlewareType { get; init; }

    public NecessaryMiddlewareNotFoundException(IRule rule, Type middleware) : base($"Missing necessary middleware '{middleware}' for rule '{rule.GetType()}' in MiddlewareCollection")
    {
        RuleType = rule.GetType();
        MiddlewareType = middleware;
    }
}

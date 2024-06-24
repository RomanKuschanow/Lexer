using Lexer.Analyzer.Middleware.Interface;

namespace Lexer.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class UseThisMiddlewareAttribute : Attribute
{
    public Type MiddlewareType { get; }
    public bool Necessary { get; }

    public UseThisMiddlewareAttribute(Type type, bool necessary = false)
    {
        if (type != typeof(IMiddleware) && type.GetInterface("IMiddleware") is null)
            throw new ArgumentException($"'{nameof(type)}' must be in the inheritance hierarchy of '{typeof(IMiddleware)}'", nameof(type));

        MiddlewareType = type;
        Necessary = necessary;
    }
}

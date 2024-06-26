using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayerFactory : IDisposable
{
    IEnumerable<IRawLayerCreator> RawLayerCreators { get; }

    public IRawLayer CreateRawLayer(Type creatorType, IEnumerable<IRawLexeme> rawLexemes, IRule rule);
}

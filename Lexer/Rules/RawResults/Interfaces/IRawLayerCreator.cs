using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayerCreator
{
    IRawLayer Create(IEnumerable<IRawLexeme> rawLexemes, IRule rule);
}

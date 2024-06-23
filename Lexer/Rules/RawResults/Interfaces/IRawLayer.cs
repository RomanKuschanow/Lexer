using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLayer
{
    public IEnumerable<IRawLexeme> RawLexemes { get; }
    public IRule Rule { get; }
}

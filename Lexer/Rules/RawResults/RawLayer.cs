using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayer : IRawLayer
{
    public IEnumerable<IRawLexeme> RawLexemes { get; init; }

    public IRule Rule { get; init; }

    public RawLayer(IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        RawLexemes = rawLexemes;
        Rule = rule;
    }
}

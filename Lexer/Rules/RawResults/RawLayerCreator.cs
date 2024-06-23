using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerCreator : IRawLayerCreator
{
    public IRawLayer Create(IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        List<RawLexeme> resultLexemes = new();

        foreach (RawLexeme lexeme in rawLexemes.OrderBy(l => l.Start))
        {
            if (resultLexemes.Any(l => l.Start <= lexeme.Start && l.Start + l.Length > lexeme.Start))
                continue;

            var lexemesAtOnePoint = rawLexemes.Where(l => l.Start == lexeme.Start);
            if (lexemesAtOnePoint.Count() > 1 && lexemesAtOnePoint.Where(l => l.Length < lexeme.Length).Any())
                continue;

            resultLexemes.Add(lexeme);
        }

        return new RawLayer(resultLexemes, rule);
    }
}

using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;
public class RawLayerCreator : IRawLayerCreator
{
    public IRawLayer Create(IEnumerable<IRawLexeme> rawLexemes, IRule rule)
    {
        // Initialize a list to store the resulting lexemes
        List<RawLexeme> resultLexemes = new();

        // Iterate over the sorted raw lexemes by their start position
        foreach (RawLexeme lexeme in rawLexemes.OrderBy(l => l.Start))
        {
            // Skip the lexeme if it overlaps with any already added lexeme
            if (resultLexemes.Any(l => l.Start <= lexeme.Start && l.Start + l.Length > lexeme.Start))
                continue;

            // Get all lexemes that start at the same position as the current lexeme
            var lexemesAtOnePoint = rawLexemes.Where(l => l.Start == lexeme.Start);

            // Skip the lexeme if there are other shorter lexemes starting at the same position
            if (lexemesAtOnePoint.Count() > 1 && lexemesAtOnePoint.Where(l => l.Length < lexeme.Length).Any())
                continue;

            // Add the lexeme to the result list
            resultLexemes.Add(lexeme);
        }

        // Create and return a new RawLayer with the resulting lexemes and the associated rule
        return new RawLayer(resultLexemes, rule);
    }

}

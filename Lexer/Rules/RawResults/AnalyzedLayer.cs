using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lexer.Rules.RawResults;
public class AnalyzedLayer : IEnumerable<RawLexeme>
{
    private readonly List<RawLexeme> _rawLexemes = new();

    public int Count => _rawLexemes.Count;

    public RawLexeme this[int index]
    {
        get => _rawLexemes[index];
        set => _rawLexemes[index] = value;
    }

    private AnalyzedLayer(IEnumerable<RawLexeme> rawLexemes) => _rawLexemes = rawLexemes.ToList();

    public static async Task<AnalyzedLayer> FromIEnumerable(IEnumerable<RawLexeme> rawLexemes)
    {
        List<RawLexeme> resultLexemes = new();

        await Task.Run(() =>
        {
            foreach (RawLexeme lexeme in rawLexemes.OrderBy(l => l.Start))
            {
                if (resultLexemes.Any(l => l.Start <= lexeme.Start && l.Start + l.Length > lexeme.Start))
                    continue;

                var lexemesAtOnePoint = rawLexemes.Where(l => l.Start == lexeme.Start);
                if (lexemesAtOnePoint.Count() > 1 && lexemesAtOnePoint.Where(l => l.Length < lexeme.Length).Any())
                    continue;

                resultLexemes.Add(lexeme);
            }
        });

        return new(resultLexemes);
    }

    public IEnumerator<RawLexeme> GetEnumerator() => _rawLexemes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

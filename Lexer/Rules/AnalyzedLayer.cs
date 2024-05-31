using System.Collections;

namespace Lexer.Rules;
public class AnalyzedLayer : IEnumerable<RawLexeme>
{
    private readonly List<RawLexeme> _rawLexemes = new();

    public int Count => _rawLexemes.Count;

    public RawLexeme this[int index]
    {
        get => _rawLexemes[index];
        set => _rawLexemes[index] = value;
    }

    public AnalyzedLayer(IEnumerable<RawLexeme> rawLexemes) => _rawLexemes = rawLexemes.ToList();

    public IEnumerator<RawLexeme> GetEnumerator() => _rawLexemes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

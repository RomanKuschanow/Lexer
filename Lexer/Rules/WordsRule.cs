#nullable disable

using System.Text.RegularExpressions;

namespace Lexer.Rules;
public class WordsRule : IRule
{
    private string _type;
    private IEnumerable<string> _words;

    public string Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value));
    }
    public bool IsIgnored { get; set; }
    public bool IsEnabled { get; set; }

    public IEnumerable<string> Words
    {
        get => _words;
        set => _words = value ?? throw new ArgumentNullException(nameof(value));
    }

    public WordsRule(IEnumerable<string> words, string type, bool isIgnored = false, bool isEnabled = true)
    {
        Words = words;
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    public async Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct = default)
    {
        var matches = Regex.Matches(str, $@"\b{string.Join('|', Words)}\b");

        return await Task.FromResult(new AnalyzedLayer(matches.Select(m => new RawLexeme(m.Index, m.Length, this))));
    }
}

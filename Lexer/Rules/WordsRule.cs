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

    /// <summary>
    /// Gets or sets the collection of words used for matching in the text.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the value is set to null.</exception>
    public IEnumerable<string> Words
    {
        get => _words;
        set => _words = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WordsRule"/> class with specified words, rule type, and settings.
    /// </summary>
    /// <param name="words">The collection of words to use for lexeme identification.</param>
    /// <param name="type">The type name of the rule.</param>
    /// <param name="isIgnored">Optional. Indicates whether lexemes found should be ignored. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Indicates whether the rule is active. Defaults to true.</param>
    public WordsRule(IEnumerable<string> words, string type, bool isIgnored = false, bool isEnabled = true)
    {
        Words = words;
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    /// <summary>
    /// Asynchronously identifies lexemes that match any of the specified words in the text.
    /// </summary>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    /// This method constructs a regex from the words and finds matches in the text.
    public async Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct = default)
    {
        var matches = Regex.Matches(str, $@"\b{string.Join('|', Words)}\b");

        return await Task.FromResult(new AnalyzedLayer(matches.Select(m => new RawLexeme(m.Index, m.Length, this))));
    }
}

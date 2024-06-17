#nullable disable

using System.Text.RegularExpressions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;

namespace Lexer.Rules.Common;
public class WordsRule : RuleBase
{
    private IEnumerable<string> _words;

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
    /// <param name="ruleSettings">The settings for the rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null words or ruleSettings is passed to the constructor.</exception>
    public WordsRule(IEnumerable<string> words, IRuleSettings ruleSettings) : base(ruleSettings)
    {
        Words = words;
    }

    /// <summary>
    /// Asynchronously identifies lexemes that match any of the specified words in the text.
    /// </summary>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    /// This method constructs a regex from the words and finds matches in the text.
    public override async Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct = default)
    {
        var matches = Regex.Matches(input.Text, $@"\b{string.Join('|', Words)}\b");

        return await AnalyzedLayer.FromIEnumerable(matches.Select(m => new RawLexeme(m.Index, m.Length, this)));
    }
}

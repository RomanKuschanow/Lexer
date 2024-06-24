#nullable disable

using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Text.RegularExpressions;

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
    public WordsRule(IEnumerable<string> words, string type, IRuleSettings ruleSettings) : base(type, ruleSettings)
    {
        Words = words;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WordsRule"/> class with specified words, rule type, and settings.
    /// </summary>
    /// <param name="words">The collection of words to use for lexeme identification.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null words or ruleSettings is passed to the constructor.</exception>
    public WordsRule(IEnumerable<string> words, string type) : this(words, type, new CommonRuleSettings()) { }

    /// <summary>
    /// Asynchronously identifies lexemes that match any of the specified words in the text.
    /// </summary>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    /// This method constructs a regex from the words and finds matches in the text.
    public override IEnumerable<IRawLexeme> FindLexemes(IRuleInput input)
    {
        var matches = Regex.Matches(input.Text, $@"\b{string.Join('|', Words)}\b");

        return matches.Select(m => new RawLexeme(m.Index, m.Length, this));
    }
}

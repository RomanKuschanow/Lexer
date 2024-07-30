#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Text.RegularExpressions;

namespace Lexer.Rules.Common;
public class RegexRule : RuleBase
{
    private Regex _regex;

    /// <summary>
    /// Gets or sets the Regex pattern used by this rule to identify lexemes in text.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when trying to set this property to null.</exception>
    /// <value>
    /// The Regex object that defines the pattern used to match text segments as lexemes.
    /// </value>
    public Regex Regex
    {
        get => _regex;
        set => _regex = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexRule"/> class using the specified regex pattern and rule type.
    /// </summary>
    /// <param name="regex">The regex pattern used to identify lexemes in text. This cannot be null.</param>
    /// <param name="ruleSettings">The settings for the rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null regex or ruleSettings is passed to the constructor.</exception>
    public RegexRule(Regex regex, string type, IRuleSettings ruleSettings) : base(type, ruleSettings)
    {
        Regex = regex;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegexRule"/> class using the specified regex pattern and rule type.
    /// </summary>
    /// <param name="regex">The regex pattern used to identify lexemes in text. This cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null regex or ruleSettings is passed to the constructor.</exception>
    public RegexRule(Regex regex, string type) : this(regex, type, new CommonRuleSettings()) { }

    /// <summary>
    /// Asynchronously finds lexemes in the specified text using the Regex pattern.
    /// </summary>
    /// <param name="input">The text to be analyzed.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    public override IEnumerable<IRawLexeme> FindLexemes(IRuleInput input)
    {
        var matches = Regex.Matches(input.Text);

        return matches.Select(m => new RawLexeme(m.Index, m.Length, this, Type));
    }
}

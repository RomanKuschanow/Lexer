#nullable disable
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Text.RegularExpressions;

namespace Lexer.Rules.Common;
public class DelimiterRule : RuleBase
{
    private Regex _startDelimiter;
    private Regex _endDelimiter;

    /// <summary>
    /// Gets or sets the regex pattern used to identify the start delimiter.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when trying to set this property to null.</exception>
    public Regex StartDelimiter
    {
        get => _startDelimiter;
        set => _startDelimiter = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the regex pattern used to identify the end delimiter.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when trying to set this property to null.</exception>
    public Regex EndDelimiter
    {
        get => _endDelimiter;
        set => _endDelimiter = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelimiterRule"/> class with specified start and end delimiters, rule type, and settings.
    /// </summary>
    /// <param name="startDelimiter">The regex pattern used to identify the start delimiter.</param>
    /// <param name="endDelimiter">The regex pattern used to identify the end delimiter.</param>
    /// <param name="ruleSettings">The settings for the rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null startDelimiter, endDelimiter, or ruleSettings is passed to the constructor.</exception>
    public DelimiterRule(Regex startDelimiter, Regex endDelimiter, string type, IRuleSettings ruleSettings) : base(type, ruleSettings)
    {
        StartDelimiter = startDelimiter;
        EndDelimiter = endDelimiter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelimiterRule"/> class with specified start and end delimiters, rule type, and settings.
    /// </summary>
    /// <param name="startDelimiter">The regex pattern used to identify the start delimiter.</param>
    /// <param name="endDelimiter">The regex pattern used to identify the end delimiter.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null startDelimiter, endDelimiter, or ruleSettings is passed to the constructor.</exception>
    public DelimiterRule(Regex startDelimiter, Regex endDelimiter, string type) : this(startDelimiter, endDelimiter, type, new CommonRuleSettings()) { }

    /// <summary>
    /// Asynchronously identifies lexemes that are enclosed by the specified start and end delimiters in the text.
    /// </summary>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    public override IEnumerable<IRawLexeme> FindLexemes(IRuleInput input)
    {
        List<RawLexeme> lexemes = new();

        int index = 0;
        while (index < input.Text.Length)
        {
            var startMatch = StartDelimiter.Match(input.Text, index);
            if (!startMatch.Success)
                break;

            var endMatch = EndDelimiter.Match(input.Text, startMatch.Index);
            if (!endMatch.Success)
                break;

            lexemes.Add(new(startMatch.Index, endMatch.Index + endMatch.Length - startMatch.Index, this));

            index = startMatch.Index + 1;
        }

        return lexemes;
    }
}

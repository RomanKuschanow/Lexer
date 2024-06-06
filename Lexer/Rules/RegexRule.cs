#nullable disable
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;

namespace Lexer.Rules;
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
    /// <param name="type">The name of the rule, which corresponds to the type of lexemes identified by this regex.</param>
    /// <param name="isIgnored">Optional. Specifies whether lexemes found by this rule should be ignored in the output. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Specifies whether this rule is enabled and should be used in the lexeme identification process. Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null regex or type is passed to the constructor.</exception>
    public RegexRule(Regex regex, string type, bool isIgnored = false, bool isEnabled = true) : base(type, isIgnored, isEnabled)
    {
        Regex = regex;
    }

    /// <summary>
    /// Asynchronously finds lexemes in the specified text using the Regex pattern.
    /// </summary>
    /// <param name="str">The text to be analyzed.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    public override async Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct = default)
    {
        var matches = Regex.Matches(input.Text);

        return await Task.FromResult(new AnalyzedLayer(matches.Select(m => new RawLexeme(m.Index, m.Length, this))));
    }
}

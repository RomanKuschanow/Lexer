#nullable disable
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Lexer.Rules;
public class DependedRegexRule : DependencyRuleBase
{
    private string _pattern;

    /// <summary>
    /// Gets or sets the regular expression pattern.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    /// <value>
    /// The string object that defines the pattern used to match text segments as lexemes.
    /// </value>
    public string Pattern
    {
        get => _pattern;
        set => _pattern = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the options for the regular expression.
    /// </summary>
    public RegexOptions Options { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependedRegexRule"/> class with the specified pattern and type.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="type">The type of the rule.</param>
    /// <param name="isIgnored">Optional. Specifies whether lexemes found by this rule should be ignored in the output. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Specifies whether this rule is enabled and should be used in the lexeme identification process. Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null pattern or type is passed to the constructor.</exception>
    public DependedRegexRule([StringSyntax("Regex")] string pattern, string type, bool isIgnored = false, bool isEnabled = true) : this(pattern, RegexOptions.None, type, isIgnored, isEnabled) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependedRegexRule"/> class with the specified pattern, options, and type.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The options for the regular expression.</param>
    /// <param name="type">The type of the rule.</param>
    /// <param name="isIgnored">Optional. Specifies whether lexemes found by this rule should be ignored in the output. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Specifies whether this rule is enabled and should be used in the lexeme identification process. Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null pattern or type is passed to the constructor.</exception>
    public DependedRegexRule([StringSyntax("Regex")] string pattern, RegexOptions options, string type, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true) : base(type, isIgnored, isOnlyForDependentRules, isEnabled)
    {
        Pattern = pattern;
        Options = options;
    }

    /// <summary>
    /// Asynchronously finds lexemes in the specified text using the Regex pattern  and the result of the rules on which the current one depends.
    /// </summary>
    /// <param name="input">The text to be analyzed.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    public override Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct = default)
    {
        var usedNamesInPattern = Regex.Matches(Pattern, @"<(\w+)>").Select(m => m.Groups[1].Value).Distinct();
        var validNames = Dependencies.SelectMany(d => d.Value).Distinct().Intersect(usedNamesInPattern);

        throw new NotImplementedException();
    }
}

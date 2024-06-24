#nullable disable
using Lexer.Exceptions;
using Lexer.Extensions;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Lexer.Rules.Depended;
public class DependedRegexRule : DependedRuleBase
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
    public RegexOptions RegexOptions { get; set; }

    /// <summary>
    /// Gets or sets the options for the rule.
    /// </summary>
    public DependedRuleOptions RuleOptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependedRegexRule"/> class with the specified pattern and type.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="type">The type of the rule.</param>
    /// <param name="ruleOptions">The options for the rule.</param>
    /// <param name="isIgnored">Optional. Specifies whether lexemes found by this rule should be ignored in the output. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Specifies whether this rule is enabled and should be used in the lexeme identification process. Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null pattern or type is passed to the constructor.</exception>
    public DependedRegexRule([StringSyntax("Regex")] string pattern, string type, DependedRegexRuleSettings ruleSettings) : this(pattern, RegexOptions.None, type, ruleSettings) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependedRegexRule"/> class with the specified pattern, options, and type.
    /// </summary>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="regexOptions">The options for the regular expression.</param>
    /// <param name="type">The type of the rule.</param>
    /// <param name="ruleOptions">The options for the rule.</param>
    /// <param name="isIgnored">Optional. Specifies whether lexemes found by this rule should be ignored in the output. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Specifies whether this rule is enabled and should be used in the lexeme identification process. Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null pattern or type is passed to the constructor.</exception>
    public DependedRegexRule([StringSyntax("Regex")] string pattern, RegexOptions regexOptions, string type, DependedRegexRuleSettings ruleSettings) : base(type, ruleSettings)
    {
        Pattern = pattern;
        RegexOptions = regexOptions;
        RuleOptions = ruleSettings.RuleOptions;
    }

    /// <summary>
    /// Asynchronously finds lexemes in the specified text using the Regex pattern  and the result of the rules on which the current one depends.
    /// </summary>
    /// <param name="input">The text to be analyzed.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes.</returns>
    public override IEnumerable<IRawLexeme> FindLexemes(IDependedRuleInput input)
    {
        if (input is not IDependedRuleInput dInput)
            throw new ArgumentException($"'input' must be an instance of 'IDependedRule'", nameof(input));

        Regex forReplace = new(@"<(\w+)>");

        var usedNamesInPattern = forReplace.Matches(Pattern)
                                           .Select(m => m.Groups[1].Value)
                                           .ToList();
        var uniqueNamesInPattern = usedNamesInPattern.Distinct();
        var validNames = Dependencies.SelectMany(d => d.Value)
                                     .Distinct()
                                     .Intersect(uniqueNamesInPattern);

        if (!validNames.ScrambledEquals(uniqueNamesInPattern))
            throw new RegexDependencyException(uniqueNamesInPattern.Except(validNames));

        var uniquePointers = uniqueNamesInPattern.ToDictionary(n => n, n => new Pointer(dInput.Dependencies[Dependencies.Single(d => d.Value.Contains(n)).Key].RawLexemes.Select(l => input.Text.Substring(l.Start, l.Length)).Distinct()));

        List<Pointer> replacePointers = new();

        for (int i = 0; i < usedNamesInPattern.Count; i++)
        {
            var name = usedNamesInPattern[i];

            if (usedNamesInPattern[..i].Contains(name))
            {
                if (!RuleOptions.HasFlag(DependedRuleOptions.AlwaysSameData))
                {
                    replacePointers.Add(uniquePointers[name].Copy());
                    continue;
                }
            }

            replacePointers.Add(uniquePointers[name]);
        }

        List<RawLexeme> lexemes = new();

        while (true)
        {
            string pattern = Pattern;

            foreach (var pointer in replacePointers)
            {
                pattern = forReplace.Replace(pattern, pointer.Current, 1);
            }

            lexemes.AddRange(Regex.Matches(input.Text, pattern).Select(m => new RawLexeme(m.Index, m.Length, this)));

            var distinctListOfPointers = replacePointers.Distinct().ToList();
            int index = 0;
            while (index < distinctListOfPointers.Count && distinctListOfPointers[index].Index == distinctListOfPointers[index].Length - 1)
            {
                distinctListOfPointers[index].Index = 0;
                index++;
            }

            if (index >= distinctListOfPointers.Count)
            {
                break;
            }

            distinctListOfPointers[index].Index++;
        }

        return lexemes;
    }

    private class Pointer
    {
        private int _index;

        public List<string> Lexemes { get; set; }

        public int Length => Lexemes.Count;

        public int Index
        {
            get => _index;
            set => _index = value >= Length ? Length - 1 : value;
        }

        public string Current => Lexemes[Index];

        public Pointer(IEnumerable<string> lexemes) => Lexemes = lexemes.ToList();

        public Pointer Copy() => new(Lexemes);
    }
}

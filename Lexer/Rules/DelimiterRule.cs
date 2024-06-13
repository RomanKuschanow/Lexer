#nullable disable
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Lexer.Rules;
public class DelimiterRule : RuleBase
{
    private Regex _startDelimiter;
    private Regex _endDelimiter;

    public Regex StartDelimiter
    {
        get => _startDelimiter;
        set => _startDelimiter = value ?? throw new ArgumentNullException(nameof(value));
    }
    public Regex EndDelimiter
    {
        get => _endDelimiter;
        set => _endDelimiter = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DelimiterRule(Regex startDelimiter, Regex endDelimiter, string type, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true) : base(type, isIgnored, isOnlyForDependentRules, isEnabled)
    {
        StartDelimiter = startDelimiter;
        EndDelimiter = endDelimiter;
    }

    public override async Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct = default)
    {
        List<RawLexeme> lexemes = new();

        await Task.Run(() =>
        {
            int index = 0;
            while (index < input.Text.Length)
            {
                var startMatch = StartDelimiter.Match(input.Text, index);
                if (!startMatch.Success)
                    break;

                var endMatch = EndDelimiter.Match(input.Text, startMatch.Index);
                if (!endMatch.Success)
                    break;

                lexemes.Add(new(startMatch.Index, (endMatch.Index + endMatch.Length) - startMatch.Index, this));

                index = startMatch.Index + 1;
            }
        }, ct);

        return await AnalyzedLayer.FromIEnumerable(lexemes);
    }
}

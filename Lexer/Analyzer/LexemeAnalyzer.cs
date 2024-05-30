#nullable disable
using Lexer.Rules;
using System.Data;

namespace Lexer.Analyzer;
public class LexemeAnalyzer
{
    public RuleSet Rules { get; set; }

    public LexemeAnalyzerOptions Options { get; set; }

    private SemaphoreSlim _semaphore;

    public LexemeAnalyzer(RuleSet rules = null!, LexemeAnalyzerOptions options = null!)
    {
        Rules = rules ?? new();
        Options = options ?? new();
    }

    public async Task<AnalyzeResult> Analyze(string str, int maxDegreeOfParallelism = 10, CancellationToken ct = default)
    {
        using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
        var tasks = Rules.Select(async r =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                return await r.FindLexemes(str, ct);
            }
            finally
            {

                semaphore.Release();
            }
        });

        var layers = await Task.WhenAll(tasks);

        List<Lexeme> lexemes = new();
        List<UnrecognizedPart> errors = new();

        await Task.Run(() =>
        {
            int startIndex = 0;
            IEnumerable<RawLexeme> candidates;
            while ((candidates = layers.Select(l => l.FirstOrDefault(r => r.Start > startIndex))
                                       .Where(l => l is not null).Cast<RawLexeme>()).Any())
            {
                var selectedLexeme = candidates.First();
                foreach (var lexeme in candidates)
                    if (lexeme.Start < selectedLexeme.Start)
                        selectedLexeme = lexeme;

                if (!selectedLexeme.Rule.IsIgnored)
                    lexemes.Add(new(selectedLexeme.Rule.Type, str.Substring(selectedLexeme.Start, selectedLexeme.Length)));

                if (selectedLexeme.Start > startIndex)
                {
                    var (ln, ch) = GetLineAndCharacter(startIndex);
                    errors.Add(new(ln, ch, selectedLexeme.Start - startIndex));
                }

                startIndex = selectedLexeme.Start + selectedLexeme.Length;
            }
        }, ct);

        return new(lexemes, errors);

        (int, int) GetLineAndCharacter(int index)
        {
            int ln = str[..index].Where(ch => ch == '\n').Count() + 1;
            int lastIndexOfNewLine = str[..(index + 1)].LastIndexOf('\n');
            int ch = index - (lastIndexOfNewLine > -1 ? lastIndexOfNewLine : 0);
            return (ln, ch);
        }
    }
}

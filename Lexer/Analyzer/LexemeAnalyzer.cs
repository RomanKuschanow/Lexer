#nullable disable
using Lexer.Rules;
using System.Data;

namespace Lexer.Analyzer;

/// <summary>
/// Analyzes a string based on a set of lexical rules.
/// </summary>
public class LexemeAnalyzer
{
    /// <summary>
    /// Gets or sets the set of rules used for analysis.
    /// </summary>
    public RuleSet Rules { get; set; }

    /// <summary>
    /// Gets or sets the options for the lexeme analyzer.
    /// </summary>
    public LexemeAnalyzerOptions Options { get; set; }

    private SemaphoreSlim _semaphore;

    /// <summary>
    /// Initializes a new instance of the <see cref="LexemeAnalyzer"/> class.
    /// </summary>
    /// <param name="rules">The set of rules used for analysis.</param>
    /// <param name="options">The options for the lexeme analyzer.</param>
    public LexemeAnalyzer(RuleSet rules = null!, LexemeAnalyzerOptions options = null!)
    {
        Rules = rules ?? new();
        Options = options ?? new();
    }

    /// <summary>
    /// Analyzes the input string and returns the analysis result.
    /// </summary>
    /// <param name="str">The input string to analyze.</param>
    /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism for the analysis.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The result of the analysis.</returns>
    public async Task<AnalyzeResult> Analyze(string str, int maxDegreeOfParallelism = 10, CancellationToken ct = default)
    {
        // Create a semaphore to limit the degree of parallelism.
        using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

        // Create tasks for each rule to find lexemes in parallel.
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

        // Wait for all tasks to complete and gather their results.
        var layers = await Task.WhenAll(tasks);

        List<Lexeme> lexemes = new();
        List<UnrecognizedPart> errors = new();

        // Process the results to identify lexemes and errors.
        await Task.Run(() =>
        {
            int startIndex = 0;
            IEnumerable<RawLexeme> candidates;

            // Loop to find and process lexemes
            while ((candidates = layers.Select(l => l.FirstOrDefault(r => r.Start >= startIndex))
                                       .Where(l => l is not null).Cast<RawLexeme>()).Any())
            {
                // Select the first lexeme candidate
                var selectedLexeme = candidates.First();

                // Find the lexeme with the earliest start position
                foreach (var lexeme in candidates.Skip(1))
                    if (lexeme.Start < selectedLexeme.Start)
                        selectedLexeme = lexeme;

                // Add the lexeme to the list if it is not ignored
                if (!selectedLexeme.Rule.IsIgnored)
                    lexemes.Add(new(selectedLexeme.Rule.Type, str.Substring(selectedLexeme.Start, selectedLexeme.Length)));

                // If there is a gap between the current start index and the lexeme's start position, record it as an error
                if (selectedLexeme.Start > startIndex)
                {
                    var (ln, ch) = GetLineAndCharacter(startIndex);
                    errors.Add(new(ln, ch, selectedLexeme.Start - startIndex));
                }

                // Update the start index to the end of the current lexeme
                startIndex = selectedLexeme.Start + selectedLexeme.Length;
            }
        }, ct);

        return new(lexemes, errors);

        // Helper method to calculate line and character positions for error reporting.
        (int, int) GetLineAndCharacter(int index)
        {
            int ln = str[..index].Where(ch => ch == '\n').Count() + 1;
            int lastIndexOfNewLine = str[..(index + 1)].LastIndexOf('\n');
            int ch = index - (lastIndexOfNewLine > -1 ? lastIndexOfNewLine : 0);
            return (ln, ch);
        }
    }
}

﻿#nullable disable
using Lexer.Rules;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;
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

    public async Task<AnalyzeResult> Analyze(string text, int maxDegreeOfParallelism = 10, CancellationToken ct = default)
    {
        await Rules.PrepareRules();

        using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

        Dictionary<IRule<IRuleInput>, AnalyzedLayer> layersDict = new();

        var tasks = Rules.Select(async r =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                if (r is IRule)
                {
                    var result = await r.FindLexemes(new RuleInput(text), ct);
                    layersDict.Add(r, result);

                    return result;
                }

                else if (r is IDependencyRule rule)
                {
                    Dictionary<IRule<IRuleInput>, AnalyzedLayer> dependencies = new();
                    foreach (var dependencyRule in rule.Dependencies)
                    {
                        dependencies.Add(dependencyRule, layersDict[dependencyRule]);
                    }
                    var input = new DependencyRuleInput(text, dependencies);

                    var result = await r.FindLexemes(input, ct);
                    layersDict.Add(r, result);

                    return result;
                }
                else
                    throw new ArgumentException("'r' has unexpected type", nameof(r));
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
            while ((candidates = layers.Select(l => l.FirstOrDefault(r => r.Start >= startIndex))
                                       .Where(l => l is not null).Cast<RawLexeme>()).Any())
            {
                var selectedLexeme = candidates.First();
                foreach (var lexeme in candidates.Skip(1))
                    if (lexeme.Start < selectedLexeme.Start)
                        selectedLexeme = lexeme;

                if (!selectedLexeme.Rule.IsIgnored)
                    lexemes.Add(new(selectedLexeme.Rule.Type, text.Substring(selectedLexeme.Start, selectedLexeme.Length)));

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
            int ln = text[..index].Where(ch => ch == '\n').Count() + 1;
            int lastIndexOfNewLine = text[..(index + 1)].LastIndexOf('\n');
            int ch = index - (lastIndexOfNewLine > -1 ? lastIndexOfNewLine : 0);
            return (ln, ch);
        }
    }
}

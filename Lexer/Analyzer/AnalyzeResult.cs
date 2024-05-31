using System.Collections.Immutable;

namespace Lexer.Analyzer;
public record AnalyzeResult
{
    public ImmutableList<Lexeme> Lexemes { get; }
    public ImmutableList<UnrecognizedPart> Errors { get; }
    public bool IsCorrect => Errors.Count == 0;

    public AnalyzeResult(IEnumerable<Lexeme> lexemes, IEnumerable<UnrecognizedPart> errors)
    {
        Lexemes = lexemes.ToImmutableList();
        Errors = errors.ToImmutableList();
    }
}

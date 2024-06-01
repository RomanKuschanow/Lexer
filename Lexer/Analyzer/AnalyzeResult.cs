using System.Collections.Immutable;

namespace Lexer.Analyzer;
/// <summary>
/// Represents the result of an analysis, including recognized lexemes and any errors.
/// </summary>
public record AnalyzeResult
{
    /// <summary>
    /// Gets the list of recognized lexemes.
    /// </summary>
    public ImmutableList<Lexeme> Lexemes { get; }

    /// <summary>
    /// Gets the list of unrecognized parts that caused errors.
    /// </summary>
    public ImmutableList<UnrecognizedPart> Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the analysis is correct (i.e., there are no errors).
    /// </summary>
    public bool IsCorrect => Errors.Count == 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalyzeResult"/> record.
    /// </summary>
    /// <param name="lexemes">The collection of recognized lexemes.</param>
    /// <param name="errors">The collection of unrecognized parts that caused errors.</param>
    public AnalyzeResult(IEnumerable<Lexeme> lexemes, IEnumerable<UnrecognizedPart> errors)
    {
        Lexemes = lexemes.ToImmutableList();
        Errors = errors.ToImmutableList();
    }
}


namespace Lexer.Rules;
public interface IRule
{
    /// <summary>
    /// Gets or sets the type of the rule, which is also passed to the created lexeme.
    /// </summary>
    string Type { get; set; }

    /// <summary>
    /// Determines whether the lexemes found by this rule should be ignored in the output.
    /// </summary>
    bool IsIgnored { get; set; }

    /// <summary>
    /// Determines whether the rule is enabled and should be used for processing the input text.
    /// Unlike IsIgnored, if a rule is disabled, the text will not be processed by this rule at all.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Identifies lexemes in the provided string based on the rule.
    /// </summary>
    /// <param name="str">The input string to be analyzed.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation and contains the analyzed lexemes.</returns>
    Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct);
}


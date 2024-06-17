#nullable disable

using Lexer;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;

namespace Lexer.Rules.Common;
public class CustomRule : RuleBase
{
    private Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> _func;

    /// <summary>
    /// Gets or sets the function used to identify lexemes in the text.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the value is set to null.</exception>
    public Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> Func
    {
        get => _func;
        set => _func = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRule"/> class with a custom function for lexeme identification.
    /// </summary>
    /// <param name="func">The function that defines how to find lexemes.</param>
    /// <param name="ruleSettings">The settings for the rule.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null func or ruleSettings is passed to the constructor.</exception>
    public CustomRule(Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> func, IRuleSettings ruleSettings) : base(ruleSettings)
    {
        Func = func;
    }

    /// <summary>
    /// Asynchronously finds lexemes in the specified text using the provided custom function.
    /// </summary>
    /// <param name="input">The text to be analyzed.</param>
    /// <param name="ct">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task that yields an AnalyzedLayer containing the identified lexemes as determined by the custom function.</returns>
    public override async Task<AnalyzedLayer> FindLexemes(IRuleInput input, CancellationToken ct = default) => await Func(input, ct, this);
}

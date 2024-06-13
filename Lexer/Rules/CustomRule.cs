#nullable disable

using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;

namespace Lexer.Rules;
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
    /// <param name="type">The type name of the rule.</param>
    /// <param name="isIgnored">Optional. Indicates whether lexemes found should be ignored. Defaults to false.</param>
    /// <param name="isEnabled">Optional. Indicates whether the rule is active. Defaults to true.</param>
    public CustomRule(Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> func, string type, bool isIgnored = false, bool isOnlyForDependentRules = false, bool isEnabled = true) : base(type, isIgnored, isOnlyForDependentRules, isEnabled)
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

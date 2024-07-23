namespace Lexer.Rules.Interfaces;
public interface IRuleSettings
{
    /// <summary>
    /// The rule will be applied only to dependent rules. For everything else, the effect of this parameter is similar to the <see cref="IsEnabled"/>
    /// </summary>
    bool IsOnlyForProcessing { get; }

    /// <summary>
    /// Determines whether the lexemes found by this rule should be ignored in the output.
    /// </summary>
    bool IsIgnored { get; }

    /// <summary>
    /// Determines whether the rule is enabled and should be used for processing the input text.
    /// Unlike <see cref="IsIgnored"/>, if a rule is disabled, the text will not be processed by this rule at all.
    /// </summary>
    bool IsEnabled { get; }
}

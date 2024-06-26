using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Collections.Immutable;

namespace Lexer.Rules.Interfaces;
public interface IDependedRule : IRule
{
    /// <summary>
    /// Gets the dependencies of the rule as an immutable dictionary.
    /// </summary>
    ImmutableDictionary<IRule, string[]> Dependencies { get; }

    /// <summary>
    /// Adds a dependency to the rule with the specified names.
    /// </summary>
    /// <param name="rule">The rule to add as a dependency.</param>
    /// <param name="names">The names associated with the dependency.</param>
    void AddDependency(IRule rule, params string[] names);

    /// <summary>
    /// Sets the dependency names for a specified rule.
    /// </summary>
    /// <param name="rule">The rule to set the dependency names for.</param>
    /// <param name="names">The names to set for the dependency.</param>
    void SetDependencyNames(IRule rule, params string[] names);

    /// <summary>
    /// Removes a dependency from the rule.
    /// </summary>
    /// <param name="rule">The rule to remove as a dependency.</param>
    void RemoveDependency(IRule rule);

    /// <summary>
    /// Clears all dependencies from the rule.
    /// </summary>
    void ClearDependencies();

    /// <summary>
    /// Finds lexemes using the specified depended rule input.
    /// </summary>
    /// <param name="input">The input for finding lexemes.</param>
    /// <returns>A collection of raw lexemes found using the input.</returns>
    IEnumerable<IRawLexeme> FindLexemes(IDependedRuleInput input);
}

namespace Lexer.Rules.Interfaces;
public interface IRuleSet : IDisposable
{
    /// <summary>
    /// Gets the collection of rules.
    /// </summary>
    IEnumerable<IRule> Rules { get; }
}

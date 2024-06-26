namespace Lexer.Rules.Interfaces;
public interface IRuleSet : IDisposable
{
    IEnumerable<IRule> Rules { get; }
}

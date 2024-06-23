using Lexer.Rules.Interfaces;

namespace Lexer.Rules.RawResults.Interfaces;
public interface IRawLexeme
{
    int Start { get; }
    int Length { get; }
    IRule Rule { get; }
}

using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Rules.RawResults;

public record RawLexeme : IRawLexeme
{    
    public int Start { get; }

    public int Length { get; }

    public IRule Rule { get; }

    public string Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RawLexeme"/> record.
    /// </summary>
    /// <param name="start">The starting position of the lexeme within the text.</param>
    /// <param name="length">The length of the lexeme within the text.</param>
    /// <param name="rule">The rule used to identify this lexeme.</param>
    public RawLexeme(int start, int length, IRule rule, string type)
    {
        Start = start;
        Length = length;
        Rule = rule;
        Type = type;
    }
}


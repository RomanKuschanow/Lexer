using System.Collections.Immutable;

namespace Lexer.Exceptions;
public class RegexDependencyException : Exception
{
    public ImmutableList<string> MissedNames { get; }

    public RegexDependencyException(IEnumerable<string> missedNames) : base($"The following names are missing in the dependency list {string.Join(", ", missedNames.Select(s => $"\"{s}\""))}")
    {
        MissedNames = missedNames.ToImmutableList();
    }
}

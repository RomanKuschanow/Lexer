namespace Lexer.Rules;
public interface IRule
{
    string Type { get; set; }
    bool IsEnabled { get; set; }
    bool IsIgnored { get; set; }

    Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct);
}

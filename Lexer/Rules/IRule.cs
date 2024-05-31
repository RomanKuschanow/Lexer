namespace Lexer.Rules;
public interface IRule
{
    string Type { get; set; }
    bool IsIgnored { get; set; }
    bool IsEnabled { get; set; }

    Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct);
}

#nullable disable

namespace Lexer.Rules;
public class CustomRule : IRule
{
    private string _type;
    private Func<string, CancellationToken, Task<AnalyzedLayer>> _func;
    public string Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value));
    }
    public bool IsIgnored { get; set; }
    public bool IsEnabled { get; set; }

    public Func<string, CancellationToken, Task<AnalyzedLayer>> Func
    {
        get => _func;
        set => _func = value ?? throw new ArgumentNullException(nameof(value));
    }

    public CustomRule(Func<string, CancellationToken, Task<AnalyzedLayer>> func, string type, bool isIgnored = false, bool isEnabled = true)
    {
        Func = func;
        Type = type;
        IsIgnored = isIgnored;
        IsEnabled = isEnabled;
    }

    public async Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct = default) => await Func(str, ct);
}

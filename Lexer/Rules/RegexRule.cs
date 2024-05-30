﻿#nullable disable
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace Lexer.Rules;
public class RegexRule : IRule
{
    private string _type;
    private Regex _regex;

    public string Type 
    { 
        get => _type;
        set => _type = value ?? throw new ArgumentNullException(nameof(value)); 
    }
    public bool IsEnabled { get; set; }
    public bool IsIgnored { get; set; }

    public Regex Regex 
    {
        get => _regex;
        set => _regex = value ?? throw new ArgumentNullException(nameof(value)); 
    }

    public RegexRule(Regex regex, string type, bool isEnabled = true, bool isIgnored = false)
    {
        Regex = regex;
        Type = type;
        IsEnabled = isEnabled;
        IsIgnored = isIgnored;
    }

    public async Task<AnalyzedLayer> FindLexemes(string str, CancellationToken ct)
    {
        var matches = Regex.Matches(str);

        return await Task.FromResult(new AnalyzedLayer(matches.Select(m => new RawLexeme(m.Index, m.Length, this))));
    }
}

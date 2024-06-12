using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;

namespace Lexer.Rules.Visitors;
public class VisitorInput
{
    public string Text { get; init; }
    public Dictionary<IRule, AnalyzedLayer> Layers { get; init; }

    public VisitorInput(string text, Dictionary<IRule, AnalyzedLayer> layers)
    {
        Text = text;
        Layers = layers;
    }
}

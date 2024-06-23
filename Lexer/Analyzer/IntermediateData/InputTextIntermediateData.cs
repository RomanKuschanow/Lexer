#nullable disable
using Lexer.Analyzer.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class InputTextIntermediateData : IIntermediateData<string>
{
    private string Text { get; set; }

    public InputTextIntermediateData(string text)
    {
        Text = text;
    }

    public string GetData() => Text;

    public void SetData(string data) => Text = data;
}

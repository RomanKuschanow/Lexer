#nullable disable
using Lexer.Analyzer.Interfaces;
using Lexer.Analyzer.IntermediateData.Interfaces;

namespace Lexer.Analyzer.IntermediateData;
public class InputTextIntermediateData : IIntermediateData<string>
{
    /// <summary>
    /// Gets or sets the text data.
    /// </summary>
    private string Text { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputTextIntermediateData"/> class with the specified text.
    /// </summary>
    /// <param name="text">The initial text data.</param>
    public InputTextIntermediateData(string text)
    {
        Text = text;
    }

    public string GetData() => Text;

    public void SetData(string data) => Text = data;

    object IIntermediateData.GetData() => Text;
    void IIntermediateData.SetData(object data) => Text = (string)data;
}

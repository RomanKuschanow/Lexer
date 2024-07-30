#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Exceptions;
using Lexer.Extensions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Data;

namespace Lexer.Analyzer;
public class LexemeAnalyzer : IDisposable
{
    /// <summary>
    /// Gets the rule set for the analyzer.
    /// </summary>
    private IRuleSet RuleSet { get; init; }
    /// <summary>
    /// Gets the middleware collection for the analyzer.
    /// </summary>
    private IMiddlewareCollection MiddlewareCollection { get; init; }
    /// <summary>
    /// Gets the rule input factory for the analyzer.
    /// </summary>
    private IRuleInputFactory RuleInputFactory { get; init; }
    /// <summary>
    /// Gets the raw layer factory for the analyzer.
    /// </summary>
    private IRawLayerFactory RawLayerFactory { get; init; }

    /// <summary>
    /// Gets the rules from the rule set.
    /// </summary>
    public IEnumerable<IRule> Rules => RuleSet.Rules;
    /// <summary>
    /// Gets the middleware from the middleware collection.
    /// </summary>
    public IDictionary<Type, IEnumerable<IMiddleware>> Middleware => MiddlewareCollection.Middleware;
    /// <summary>
    /// Gets the rule input creators from the rule input factory.
    /// </summary>
    public IEnumerable<IRuleInputCreator> RuleInputCreators => RuleInputFactory.RuleInputCreators;
    /// <summary>
    /// Gets the raw layer creators from the raw layer factory.
    /// </summary>
    public IEnumerable<IRawLayerCreator> RawLayerCreators => RawLayerFactory.RawLayerCreators;

    /// <summary>
    /// Initializes a new instance of the <see cref="LexemeAnalyzer"/> class.
    /// </summary>
    /// <param name="ruleSet">The rule set to use.</param>
    /// <param name="middleware">The middleware collection to use.</param>
    /// <param name="ruleInputFactory">The rule input factory to use.</param>
    /// <param name="rawLayerFactory">The raw layer factory to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
    public LexemeAnalyzer(IRuleSet ruleSet, IMiddlewareCollection middleware, IRuleInputFactory ruleInputFactory, IRawLayerFactory rawLayerFactory)
    {
        RuleSet = ruleSet ?? throw new ArgumentNullException(nameof(ruleSet));
        MiddlewareCollection = middleware ?? throw new ArgumentNullException(nameof(middleware));
        RuleInputFactory = ruleInputFactory ?? throw new ArgumentNullException(nameof(ruleInputFactory));
        RawLayerFactory = rawLayerFactory ?? throw new ArgumentNullException(nameof(rawLayerFactory));
    }

    /// <summary>
    /// Analyzes the given text using the specified rules, middleware, and factories.
    /// </summary>
    /// <param name="text">The text to analyze.</param>
    /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism to use.</param>
    /// <param name="ct">The cancellation token to use for canceling the operation.</param>
    /// <returns>The result of the analysis.</returns>
    /// <exception cref="NecessaryAttributeNotFoundException">Thrown when a necessary attribute is not found.</exception>
    public AnalyzeResult Analyze(string text)
    {
        Dictionary<IRule, RawLayer> layersDict = [];
        IntermediateDataCollection intermediateDataCollection = new();
        intermediateDataCollection.Add(new InputTextIntermediateData(text));

        // Analyzing rules
        var layers = RuleSet.Rules
        .Select(rule =>
        {
            // Create input using the rule input factory
            var input = RuleInputFactory.CreateInput(rule, intermediateDataCollection);

            // Find lexemes using the rule
            var rawLexemes = rule.FindLexemes(input);

            // Create raw layer using the raw layer factory
            var layer = RawLayerFactory.CreateRawLayer(rule, rawLexemes);

            // Execute middleware for the rule
            MiddlewareCollection.Get(rule).ForEach(middleware => middleware.Execute(rule, input, layer, intermediateDataCollection));

            return layer;
        })
        .Where(layer => !layer.Rule.IsOnlyForProcessing);

        List<Lexeme> lexemes = [];
        List<UnrecognizedPart> errors = [];

        int startIndex = 0;
        IEnumerable<IRawLexeme> candidates;

        // Process lexemes and collect errors
        while ((candidates = layers.Select(layer => layer.RawLexemes.FirstOrDefault(lexeme => lexeme.Start >= startIndex))
                                   .Where(l => l is not null)
                                   .Cast<RawLexeme>())
                                   .Any())
        {
            // Select the first candidate lexeme
            var selectedLexeme = candidates.First();

            // Find the lexeme with the smallest start index
            foreach (var lexeme in candidates.Skip(1))
            {
                if (lexeme.Start < selectedLexeme.Start)
                {
                    selectedLexeme = lexeme;
                }
            }

            // Add the selected lexeme to the list if it's not ignored
            if (!selectedLexeme.Rule.IsIgnored)
            {
                lexemes.Add(new(selectedLexeme.Type, text.Substring(selectedLexeme.Start, selectedLexeme.Length), selectedLexeme.Start, selectedLexeme.Length));
            }

            // If there is a gap between the current startIndex and the start of the selected lexeme, record an error
            if (selectedLexeme.Start > startIndex)
            {
                var (ln, ch) = GetLineAndCharacter(startIndex);
                errors.Add(new(ln, ch, selectedLexeme.Start - startIndex));
            }

            // Update startIndex to the end of the selected lexeme
            startIndex = selectedLexeme.Start + selectedLexeme.Length;
        }


        if (startIndex < text.Length)
        {
            var (ln, ch) = GetLineAndCharacter(startIndex);
            errors.Add(new(ln, ch, text.Length - startIndex));
        }

        return new(lexemes, errors);

        // Get line and character position from index
        (int, int) GetLineAndCharacter(int index)
        {
            int ln = text[..index].Where(ch => ch == '\n').Count();
            int lastIndexOfNewLine = text[..(index + 1)].LastIndexOf('\n');
            int ch = index - (lastIndexOfNewLine > -1 ? lastIndexOfNewLine : 0);
            return (ln, ch);
        }
    }

    /// <summary>
    /// Disposes the lexeme analyzer and its resources.
    /// </summary>
    public void Dispose()
    {
        RuleSet.Dispose();
        MiddlewareCollection.Dispose();
        RuleInputFactory.Dispose();
        RawLayerFactory.Dispose();
        GC.SuppressFinalize(this);
    }
}

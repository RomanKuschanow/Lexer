#nullable disable
using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.Middleware;
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Attributes;
using Lexer.Exceptions;
using Lexer.Extensions;
using Lexer.Rules;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Data;

namespace Lexer.Analyzer;
public class LexemeAnalyzer
{
    private RuleSet RuleSet { get; init; }
    private MiddlewareCollection MiddlewareCollection { get; init; }
    private RuleInputFactory RuleInputFactory { get; init; }
    private RawLayerFactory RawLayerFactory { get; init; }

    public IEnumerable<IRule> Rules => RuleSet.Rules;
    public IEnumerable<IMiddleware> Middleware => MiddlewareCollection.Middleware;
    public IEnumerable<IRuleInputCreator> RuleInputCreators => RuleInputFactory.RuleInputCreators;
    public IEnumerable<IRawLayerCreator> RawLayerCreators => RawLayerFactory.RawLayerCreators;

    public LexemeAnalyzer(RuleSet ruleSet, MiddlewareCollection middleware, RuleInputFactory ruleInputFactory, RawLayerFactory rawLayerFactory)
    {
        RuleSet = ruleSet ?? throw new ArgumentNullException(nameof(ruleSet));
        MiddlewareCollection = middleware ?? throw new ArgumentNullException(nameof(middleware));
        RuleInputFactory = ruleInputFactory ?? throw new ArgumentNullException(nameof(ruleInputFactory));
        RawLayerFactory = rawLayerFactory ?? throw new ArgumentNullException(nameof(rawLayerFactory));
    }

    public async Task<AnalyzeResult> Analyze(string text, int maxDegreeOfParallelism = 10, CancellationToken ct = default)
    {
        RuleSet.PrepareRules();

        Dictionary<IRule, RawLayer> layersDict = [];
        IntermediateDataCollection intermediateDataCollection = new();
        intermediateDataCollection.Add(new InputTextIntermediateData(text));

        var layers = RuleSet.Rules
        .AsParallel()
        .WithDegreeOfParallelism(maxDegreeOfParallelism)
        .WithCancellation(ct)
        .AsOrdered()
        .Select(rule =>
        {
            var input = RuleInputFactory.CreateInput(GetAttributeValue<UseThisRuleInputCreatorAttribute>(rule).Type, intermediateDataCollection);

            var rawLexemes = rule.FindLexemes(input);

            var layer = RawLayerFactory.CreateRawLayer(GetAttributeValue<UseThisRawLayerCreatorAttribute>(rule).Type, rawLexemes, rule);

            MiddlewareCollection.GetMiddlewareByRule(rule).ForEach(middleware => middleware.Execute(rule, input, layer, intermediateDataCollection));

            return layer;
        })
        .Where(layer => !layer.Rule.IsOnlyForDependentRules);

        List<Lexeme> lexemes = [];
        List<UnrecognizedPart> errors = [];

        await Task.Run(() =>
        {
            int startIndex = 0;
            IEnumerable<IRawLexeme> candidates;
            while ((candidates = layers.Select(layer => layer.RawLexemes.FirstOrDefault(lexeme => lexeme.Start >= startIndex))
                                       .Where(l => l is not null).Cast<RawLexeme>()).Any())
            {
                var selectedLexeme = candidates.First();
                foreach (var lexeme in candidates.Skip(1))
                    if (lexeme.Start < selectedLexeme.Start)
                        selectedLexeme = lexeme;

                if (!selectedLexeme.Rule.IsIgnored)
                    lexemes.Add(new(selectedLexeme.Rule.Type, text.Substring(selectedLexeme.Start, selectedLexeme.Length)));

                if (selectedLexeme.Start > startIndex)
                {
                    var (ln, ch) = GetLineAndCharacter(startIndex);
                    errors.Add(new(ln, ch, selectedLexeme.Start - startIndex));
                }

                startIndex = selectedLexeme.Start + selectedLexeme.Length;
            }

            if (startIndex < text.Length)
            {
                var (ln, ch) = GetLineAndCharacter(startIndex);
                errors.Add(new(ln, ch, text.Length - startIndex));
            }
        }, ct);

        return new(lexemes, errors);

        (int, int) GetLineAndCharacter(int index)
        {
            int ln = text[..index].Where(ch => ch == '\n').Count() + 1;
            int lastIndexOfNewLine = text[..(index + 1)].LastIndexOf('\n');
            int ch = index - (lastIndexOfNewLine > -1 ? lastIndexOfNewLine : 0) + 1;
            return (ln, ch);
        }
    }

    private static T GetAttributeValue<T>(IRule rule) where T : Attribute
    {
        var type = rule.GetType();

        if (type.GetCustomAttributes(typeof(T), true).FirstOrDefault() is T attribute)
            return attribute;

        throw new NecessaryAttributeNotFoundException(rule, typeof(T));
    }
}

using Lexer.Analyzer;
using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using System.Security.AccessControl;

namespace Lexer.Tests.Analyzer;
public class LexemeAnalyzerFixtures
{
    private (IEnumerable<IRule>, IEnumerable<IRule>) GetRules()
    {
        List<IRule> executedRules = new();

        var rule1 = new Mock<IRule>();
        rule1.SetupGet(x => x.IsEnabled).Returns(true);
        rule1.SetupGet(x => x.IsOnlyForProcessing).Returns(false);
        rule1.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Callback(() => executedRules.Add(rule1.Object));

        var rule2 = new Mock<IDependedRule>();
        rule2.SetupGet(x => x.IsEnabled).Returns(true);
        rule2.SetupGet(x => x.IsOnlyForProcessing).Returns(false);
        rule2.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Callback(() => executedRules.Add(rule2.Object));

        var rule3 = new Mock<IRule>();
        rule3.SetupGet(x => x.IsEnabled).Returns(true);
        rule3.SetupGet(x => x.IsOnlyForProcessing).Returns(false);
        rule3.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Callback(() => executedRules.Add(rule3.Object));

        return ([rule1.Object, rule2.Object, rule3.Object], executedRules);
    }

    private (IMiddlewareCollection, IEnumerable<int>) GetMiddlewareCollection()
    {
        List<int> executedTimes = [];

        var middlewareCollection = new Mock<IMiddlewareCollection>();
        middlewareCollection.Setup(x => x.Get(It.IsAny<IRule>())).Callback(() => executedTimes.Add(1));

        return (middlewareCollection.Object, executedTimes);
    }

    private (IRuleInputFactory, IEnumerable<int>) GetRuleInputFactory()
    {
        List<int> executedTimes = [];

        var inputFactory = new Mock<IRuleInputFactory>();
        inputFactory.Setup(x => x.CreateInput(It.IsAny<IRule>(), It.IsAny<IIntermediateDataCollection>())).Callback(() => executedTimes.Add(1));

        return (inputFactory.Object, executedTimes);
    }

    private (IRawLayerFactory, IEnumerable<int>) GetRawLayerFactory()
    {
        List<int> executedTimes = [];

        var rawLayerFactory = new Mock<IRawLayerFactory>();
        rawLayerFactory.Setup(x => x.CreateRawLayer(It.IsAny<IRule>(), It.IsAny<IEnumerable<IRawLexeme>>())).Returns((IRule rule, IEnumerable<IRawLexeme> _) =>
        {
            var layer = new Mock<IRawLayer>();
            layer.SetupGet(x => x.RawLexemes).Returns([]);
            layer.SetupGet(x => x.Rule).Returns(rule);
            return layer.Object;
        }).Callback(() => executedTimes.Add(1));

        return (rawLayerFactory.Object, executedTimes);
    }

    [Fact]
    public void GivenRules_WhenAnalyze_ThenAllRulesExecutedInCorrectOrder()
    {
        // Arrange
        var (rules, executedRules) = GetRules();

        var ruleSet = new Mock<IRuleSet>();
        ruleSet.SetupGet(x => x.Rules).Returns(rules);

        var (layerFactory, _) = GetRawLayerFactory();

        LexemeAnalyzer sut = new(ruleSet.Object, Mock.Of<IMiddlewareCollection>(), Mock.Of<IRuleInputFactory>(), layerFactory);

        // Act
        sut.Analyze("");

        // Assert
        executedRules.Should().ContainInConsecutiveOrder(ruleSet.Object.Rules);
    }

    [Fact]
    public void GivenInputFactory_WhenAnalyze_ThenCreateExecutedAllTimes()
    {
        // Arrange
        var (rules, executedRules) = GetRules();

        var ruleSet = new Mock<IRuleSet>();
        ruleSet.SetupGet(x => x.Rules).Returns(rules);

        var (layerFactory, _) = GetRawLayerFactory();
        var (inputFactory, count) = GetRuleInputFactory();

        LexemeAnalyzer sut = new(ruleSet.Object, Mock.Of<IMiddlewareCollection>(), inputFactory, layerFactory);

        // Act
        sut.Analyze("");

        // Assert
        count.Count().Should().Be(rules.Count());
    }

    [Fact]
    public void GivenRawLayerFactory_WhenAnalyze_ThenCreateExecutedAllTimes()
    {
        // Arrange
        var (rules, executedRules) = GetRules();

        var ruleSet = new Mock<IRuleSet>();
        ruleSet.SetupGet(x => x.Rules).Returns(rules);

        var (layerFactory, count) = GetRawLayerFactory();

        LexemeAnalyzer sut = new(ruleSet.Object, Mock.Of<IMiddlewareCollection>(), Mock.Of<IRuleInputFactory>(), layerFactory);

        // Act
        sut.Analyze("");

        // Assert
        count.Count().Should().Be(rules.Count());
    }

    [Fact]
    public void GivenMiddlewareCollection_WhenAnalyze_ThenGetExecutedAllTimes()
    {
        // Arrange
        var (rules, executedRules) = GetRules();

        var ruleSet = new Mock<IRuleSet>();
        ruleSet.SetupGet(x => x.Rules).Returns(rules);

        var (layerFactory, _) = GetRawLayerFactory();
        var (middlewareCollection, count) = GetMiddlewareCollection();

        LexemeAnalyzer sut = new(ruleSet.Object, middlewareCollection, Mock.Of<IRuleInputFactory>(), layerFactory);

        // Act
        sut.Analyze("");

        // Assert
        count.Count().Should().Be(rules.Count());
    }

    [Fact]
    public void GivenString_WhenAnalyze_ThenResultContainExpectedLexemesAndErrors()
    {
        // Arrange
        string text = "class A { }";

        var rule1 = new Mock<IRule>();
        rule1.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Returns([new RawLexeme(0, 5, rule1.Object, "1")]);
        
        var rule2 = new Mock<IRule>();
        rule2.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Returns([
            new RawLexeme(0, 5, rule2.Object, "2"),
            new RawLexeme(6, 1, rule2.Object, "2")
            ]);

        var rule3 = new Mock<IRule>();
        rule3.Setup(x => x.FindLexemes(It.IsAny<IRuleInput>())).Returns([
            new RawLexeme(8, 1, rule3.Object, "3"),
            new RawLexeme(10, 1, rule3.Object, "3")
            ]);

        var ruleSet = new Mock<IRuleSet>();
        ruleSet.SetupGet(x => x.Rules).Returns([rule1.Object, rule2.Object, rule3.Object]);

        var rawLayerFactory = new Mock<IRawLayerFactory>();
        rawLayerFactory.Setup(x => x.CreateRawLayer(It.IsAny<IRule>(), It.IsAny<IEnumerable<IRawLexeme>>())).Returns((IRule rule, IEnumerable<IRawLexeme> lexemes) =>
        {
            var layer = new Mock<IRawLayer>();
            layer.SetupGet(x => x.Rule).Returns(rule);
            layer.SetupGet(x => x.RawLexemes).Returns(lexemes);
            return layer.Object;
        });

        LexemeAnalyzer sut = new(ruleSet.Object, Mock.Of<IMiddlewareCollection>(), Mock.Of<IRuleInputFactory>(), rawLayerFactory.Object);

        // Act
        var result = sut.Analyze(text);

        // Assert
        result.Lexemes.Count.Should().Be(4);
        result.Lexemes.Select(l => l.Type).Should().BeEquivalentTo(["1", "2", "3", "3"]);
        result.IsCorrect.Should().BeFalse();
        result.Errors.Count.Should().Be(3);
        result.Errors.Select(e => e.Ln).Should().BeEquivalentTo([0, 0, 0]);
        result.Errors.Select(e => e.Ch).Should().BeEquivalentTo([5, 7, 9]);
    }
}

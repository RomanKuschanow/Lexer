using FluentAssertions;
using Lexer.Analyzer;
using Lexer.Rules;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.Visitors;
using Moq;

namespace Lexer.Tests;
public class LexemeAnalyzerFixtures
{
    [Fact]
    public async Task GivenValidRules_WhenAnalyzingText_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();

        var mockRuleInput1 = new Mock<IRuleInput>();
        var mockRuleInput2 = new Mock<IRuleInput>();

        mockRule1.Setup(r => r.Accept(It.IsAny<IVisitor>(), It.IsAny<VisitorInput>()))
                 .Returns(mockRuleInput1.Object);

        mockRule1.Setup(r => r.FindLexemes(It.IsAny<IRuleInput>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>
                 {
                     new RawLexeme(0, 4, mockRule1.Object) // test
                 }));
        mockRule1.SetupProperty(r => r.IsEnabled, true);
        mockRule1.SetupProperty(r => r.Type, "test");

        mockRule2.Setup(r => r.Accept(It.IsAny<IVisitor>(), It.IsAny<VisitorInput>()))
                 .Returns(mockRuleInput2.Object);

        mockRule2.Setup(r => r.FindLexemes(It.IsAny<IRuleInput>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>
                 {
                     new RawLexeme(5, 3, mockRule2.Object) // foo
                 }));
        mockRule2.SetupProperty(r => r.IsEnabled, true);
        mockRule2.SetupProperty(r => r.Type, "foo");

        var rules = new RuleSet { mockRule1.Object, mockRule2.Object };
        var analyzer = new LexemeAnalyzer(rules);

        string text = "test foo";

        // Act
        var result = await analyzer.Analyze(text);

        // Assert
        result.Lexemes.Should().HaveCount(2);
        result.Lexemes.Should().Contain(lexeme => lexeme.Type == "test" && lexeme.Value == "test");
        result.Lexemes.Should().Contain(lexeme => lexeme.Type == "foo" && lexeme.Value == "foo");
    }

    [Fact]
    public async Task GivenValidRules_WhenAnalyzingTextWithUnrecognizedParts_ThenReturnsExpectedErrors()
    {
        // Arrange
        var mockRule = new Mock<IRule>();

        var mockRuleInput = new Mock<IRuleInput>();

        mockRule.Setup(r => r.Accept(It.IsAny<IVisitor>(), It.IsAny<VisitorInput>()))
                .Returns(mockRuleInput.Object);

        mockRule.Setup(r => r.FindLexemes(It.IsAny<IRuleInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>
                {
                    new RawLexeme(0, 4, mockRule.Object) // test
                }));
        mockRule.SetupProperty(r => r.IsEnabled, true);

        var rules = new RuleSet { mockRule.Object };
        var analyzer = new LexemeAnalyzer(rules);

        string text = "test unknown";

        // Act
        var result = await analyzer.Analyze(text);

        // Assert
        result.Lexemes.Should().HaveCount(1);
        result.Lexemes.Should().Contain(lexeme => lexeme.Value == "test");

        result.Errors.Should().HaveCount(1);
        result.Errors.First().Ln.Should().Be(1);
        result.Errors.First().Ch.Should().Be(5);
        result.Errors.First().Length.Should().Be(8);
    }

    [Fact]
    public async Task GivenEmptyRules_WhenAnalyzingText_ThenReturnsNoLexemes()
    {
        // Arrange
        var rules = new RuleSet();
        var analyzer = new LexemeAnalyzer(rules);

        string text = "test foo";

        // Act
        var result = await analyzer.Analyze(text);

        // Assert
        result.Lexemes.Should().BeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Ln.Should().Be(1);
        result.Errors.First().Ch.Should().Be(1);
        result.Errors.First().Length.Should().Be(8);
    }

    [Fact]
    public async Task GivenRules_WhenAnalyzingTextWithMaxDegreeOfParallelism_ThenExecutesWithinLimit()
    {
        // Arrange
        var mockRule = new Mock<IRule>();

        var mockRuleInput = new Mock<IRuleInput>();

        mockRule.Setup(r => r.Accept(It.IsAny<IVisitor>(), It.IsAny<VisitorInput>()))
                .Returns(mockRuleInput.Object);

        mockRule.Setup(r => r.FindLexemes(It.IsAny<IRuleInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>
                {
                    new RawLexeme(0, 4, mockRule.Object) // test
                }));
        mockRule.SetupProperty(r => r.IsEnabled, true);

        var rules = new RuleSet { mockRule.Object };
        var analyzer = new LexemeAnalyzer(rules);

        string text = "test foo";

        // Act
        var result = await analyzer.Analyze(text, 1);

        // Assert
        result.Lexemes.Should().HaveCount(1);
        result.Lexemes.Should().Contain(lexeme => lexeme.Value == "test");
    }
}

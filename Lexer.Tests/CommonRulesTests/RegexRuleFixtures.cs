using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.Visitors;
using Moq;
using System.Text.RegularExpressions;

namespace Lexer.Tests.RulesTests;
public class RegexRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        var regex = new Regex(@"\w+");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        var sut = new RegexRule(regex, mockRuleSettings.Object);

        // Assert
        sut.Type.Should().Be("word");
        sut.IsIgnored.Should().BeFalse();
        sut.IsOnlyForDependentRules.Should().BeFalse();
        sut.IsEnabled.Should().BeTrue();
        sut.Regex.Should().Be(regex);
    }

    [Fact]
    public void GivenNullRegex_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        Action act = () => new RegexRule(null, mockRuleSettings.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var regex = new Regex(@"\w+");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new RegexRule(regex, mockRuleSettings.Object);

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("This is a test.");

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 4); // This
        result.Should().Contain(lexeme => lexeme.Start == 5 && lexeme.Length == 2); // is
        result.Should().Contain(lexeme => lexeme.Start == 8 && lexeme.Length == 1); // a
        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 4); // test
    }

    [Fact]
    public void GivenVisitorAndVisitorInput_WhenAccept_ThenCallsVisitorRule()
    {
        // Arrange
        var regex = new Regex(@"\w+");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new RegexRule(regex, mockRuleSettings.Object);

        var mockVisitor = new Mock<IVisitor>();
        var visitorInput = new VisitorInput("sample text", new Dictionary<IRule, AnalyzedLayer>());

        // Act
        sut.Accept(mockVisitor.Object, visitorInput);

        // Assert
        mockVisitor.Verify(v => v.Rule(visitorInput), Times.Once);
    }
}

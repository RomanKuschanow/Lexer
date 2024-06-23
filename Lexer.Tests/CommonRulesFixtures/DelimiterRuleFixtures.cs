using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;
using System.Text.RegularExpressions;

namespace Lexer.Tests.CommonRulesTests;
public class DelimiterRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var endDelimiter = new Regex(@"\}");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("delimiter");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        var sut = new DelimiterRule(startDelimiter, endDelimiter, mockRuleSettings.Object);

        // Assert
        sut.Type.Should().Be("delimiter");
        sut.IsIgnored.Should().BeFalse();
        sut.IsOnlyForDependentRules.Should().BeFalse();
        sut.IsEnabled.Should().BeTrue();
        sut.StartDelimiter.Should().Be(startDelimiter);
        sut.EndDelimiter.Should().Be(endDelimiter);
    }

    [Fact]
    public void GivenNullStartDelimiter_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        var endDelimiter = new Regex(@"\}");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("delimiter");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        Action act = () => new DelimiterRule(null, endDelimiter, mockRuleSettings.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullEndDelimiter_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("delimiter");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        Action act = () => new DelimiterRule(startDelimiter, null, mockRuleSettings.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var endDelimiter = new Regex(@"\}");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("delimiter");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new DelimiterRule(startDelimiter, endDelimiter, mockRuleSettings.Object);

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("This is a {test} sample.");

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 6); // {test}
    }

    [Fact]
    public void GivenVisitorAndVisitorInput_WhenAccept_ThenCallsVisitorRule()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var endDelimiter = new Regex(@"\}");
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("delimiter");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new DelimiterRule(startDelimiter, endDelimiter, mockRuleSettings.Object);

        var mockVisitor = new Mock<IRuleVisitor>();
        var visitorInput = new VisitorInput("sample text", new Dictionary<IRule, RawLayer>());

        // Act
        sut.Accept(mockVisitor.Object, visitorInput);

        // Assert
        mockVisitor.Verify(v => v.Rule(visitorInput), Times.Once);
    }
}

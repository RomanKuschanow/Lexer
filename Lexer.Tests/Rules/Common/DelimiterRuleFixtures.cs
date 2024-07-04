using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;
using System.Text.RegularExpressions;

namespace Lexer.Tests.Rules.Common;
public class DelimiterRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var endDelimiter = new Regex(@"\}");

        // Act
        var sut = new DelimiterRule(startDelimiter, endDelimiter, "delimiter");

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

        // Act
        Action act = () => _ = new DelimiterRule(null, endDelimiter, "delimiter");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullEndDelimiter_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");

        // Act
        Action act = () => _ = new DelimiterRule(startDelimiter, null, "delimiter");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var startDelimiter = new Regex(@"\{");
        var endDelimiter = new Regex(@"\}");

        var sut = new DelimiterRule(startDelimiter, endDelimiter, "delimiter");

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("This is a {test} sample.");

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 6); // {test}
    }
}

using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;
using System.Text.RegularExpressions;

namespace Lexer.Tests.Rules.Common;
public class RegexRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        var regex = new Regex(@"\w+");

        // Act
        var sut = new RegexRule(regex, "word");

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
        Regex regex = null!;

        // Act
        Action act = () => _ = new RegexRule(regex, "word");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var regex = new Regex(@"\w+");

        var sut = new RegexRule(regex, "word");

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("This is a test.");

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 4); // This
        result.Should().Contain(lexeme => lexeme.Start == 5 && lexeme.Length == 2); // is
        result.Should().Contain(lexeme => lexeme.Start == 8 && lexeme.Length == 1); // a
        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 4); // test
    }
}

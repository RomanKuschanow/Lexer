using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;

namespace Lexer.Tests.Rules.Common;
public class WordsRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        var words = new List<string> { "word1", "word2" };

        // Act
        var sut = new WordsRule(words, "word");

        // Assert
        sut.Type.Should().Be("word");
        sut.IsIgnored.Should().BeFalse();
        sut.IsOnlyForDependentRules.Should().BeFalse();
        sut.IsEnabled.Should().BeTrue();
        sut.Words.Should().BeEquivalentTo(words);
    }

    [Fact]
    public void GivenNullWords_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange

        // Act
        Action act = () => _ = new WordsRule(null, "word");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var words = new List<string> { "test", "sample" };

        var sut = new WordsRule(words, "word");

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("This is a test sample.");

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 4); // test
        result.Should().Contain(lexeme => lexeme.Start == 15 && lexeme.Length == 6); // sample
    }
}

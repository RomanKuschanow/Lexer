using FluentAssertions;
using Lexer.Rules;
using Lexer.Rules.RuleInputs;

namespace Lexer.Tests;
public class WordsRuleFixtures
{
    private List<string> _words = new()
    {
        "private",
        "public",
        "class"
    };

    [Theory]
    [InlineData("private class abc", 2)]
    [InlineData("public void Get()", 1)]
    [InlineData("3 63", 0)]
    private async Task GivenStringWithPatterns_WhenApplyWordsRule_ThenAllPatternsFound(string str, int count)
    {
        // Arrange
        var sut = new WordsRule(_words, "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(count);
    }

    [Theory]
    [InlineData("5 public ", 2, 6)]
    [InlineData("private string abc", 0, 7)]
    [InlineData("internal class abc", 9, 5)]
    private async Task GivenStringWithOnePattern_WhenApplyWordsRule_ThenStartAndLengthOfLexemeMatchesWithGivenValues(string str, int start, int length)
    {
        // Arrange
        var sut = new WordsRule(_words, "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(1);
        layer[0].Start.Should().Be(start);
        layer[0].Length.Should().Be(length);
    }
}

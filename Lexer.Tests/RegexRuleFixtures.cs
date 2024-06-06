using FluentAssertions;
using Lexer.Rules;
using Lexer.Rules.RuleInputs;

namespace Lexer.Tests;
public class RegexRuleFixtures
{
    [Theory]
    [InlineData("a b c", @"\b[A-Za-z]\b", 3)]
    [InlineData("abc abc", @"\b[A-Za-z]+\b", 2)]
    [InlineData("3 63", @"\b\d+\b", 2)]
    private async Task GivenStringWithPatterns_WhenApplyRegexRule_ThenAllPatternsFound(string str, string pattern, int count)
    {
        // Arrange
        var sut = new RegexRule(new(pattern), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(count);
    }

    [Theory]
    [InlineData("5 b ", @"\b[A-Za-z]\b", 2, 1)]
    [InlineData("654 abc", @"\b[A-Za-z]+\b", 4, 3)]
    [InlineData("3 63", @"\b\d{2}\b", 2, 2)]
    private async Task GivenStringWithOnePattern_WhenApplyRegexRule_ThenStartAndLengthOfLexemeMatchesWithGivenValues(string str, string pattern, int start, int length)
    {
        // Arrange
        var sut = new RegexRule(new(pattern), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(1);
        layer[0].Start.Should().Be(start);
        layer[0].Length.Should().Be(length);
    }
}

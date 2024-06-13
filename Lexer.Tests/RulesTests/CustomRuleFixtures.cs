using FluentAssertions;
using Lexer.Rules;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs;

namespace Lexer.Tests.RulesTests;
public class CustomRuleFixtures
{
    [Theory]
    [InlineData("1234", 4)]
    [InlineData("123", 3)]
    [InlineData("12", 2)]
    private async Task GivenStringWithPatterns_WhenApplyCustomRule_ThenAllPatternsFound(string str, int count)
    {
        // Arrange
        var sut = new CustomRule(async (s, ct, t) => await AnalyzedLayer.FromIEnumerable(s.Text.Select((ch, i) => new RawLexeme(i, 1, t))), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(count);
    }

    [Theory]
    [InlineData("    adc  ", 4, 3)]
    [InlineData("   abcde", 3, 5)]
    [InlineData("  ab      ", 2, 2)]
    private async Task GivenStringWithOnePattern_WhenApplyCustomRule_ThenStartAndLengthOfLexemeMatchesWithGivenValues(string str, int start, int length)
    {
        // Arrange
        var sut = new CustomRule(async (s, ct, t) => await AnalyzedLayer.FromIEnumerable(new RawLexeme[] { new(s.Text.IndexOf('a'), s.Text.Trim().Length, t) }), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(str));

        // Assert
        layer.Count.Should().Be(1);
        layer[0].Start.Should().Be(start);
        layer[0].Length.Should().Be(length);
    }
}

using FluentAssertions;
using Lexer.Rules;
using Lexer.Rules.RuleInputs;
using System.Text.RegularExpressions;

namespace Lexer.Tests.RulesTests;
public class DelimiterRuleFixtures
{
    [Theory]
    [InlineData("""
                //comment
                int a = 0;
                //comment
                """, "//", "(.(?=\n))|$", 2)]
    [InlineData("""
                int a = 0;
                /*
                long
                comment
                */
                """, @"\/\*", @"\*\/", 1)]
    private async Task GivenStringWithPatterns_WhenApplyDelimiterRule_ThenAllPatternsFound(string str, string start, string end, int count)
    {
        // Arrange
        var sut = new DelimiterRule(new(start), new(end), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(string.Join("", str.Where(ch => ch != '\r'))));

        // Assert
        layer.Count.Should().Be(count);
    }

    [Theory]
    [InlineData("""
                //comment
                int a = 0;
                """, "//", "(.(?=\n))|$", 0, 9)]
    [InlineData("""
                int a = 0;
                //comment
                """, "//", "(.(?=\n))|$", 11, 9)]
    [InlineData("""
                int a = 0;
                /*
                long
                comment
                */
                """, @"\/\*", @"\*\/", 11, 18)]
    private async Task GivenStringWithOnePattern_WhenApplyDelimiterRule_ThenStartAndLengthOfLexemeMatchesWithGivenValues(string str, string start, string end, int startIndex, int length)
    {
        // Arrange
        var sut = new DelimiterRule(new(start), new(end), "");

        // Act
        var layer = await sut.FindLexemes(new RuleInput(string.Join("", str.Where(ch => ch != '\r'))));

        // Assert
        layer[0].Start.Should().Be(startIndex);
        layer[0].Length.Should().Be(length);
    }
}

﻿using FluentAssertions;
using Lexer.Rules;

namespace Lexer.Tests;
public class CustomRuleFixtures
{
    [Theory]
    [InlineData("1234", 4)]
    [InlineData("123", 3)]
    [InlineData("12", 2)]
    private async Task GivenStringWithPatterns_WhenApplyCustomRule_ThenAllPatternsFound(string str, int count)
    {
        // Arrange
        var sut = new CustomRule(async (s, ct, t) => await Task.FromResult(new AnalyzedLayer(s.Select((ch, i) => new RawLexeme(i, 1, t)))), "");

        // Act
        var layer = await sut.FindLexemes(str);

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
        var sut = new CustomRule(async (s, ct, t) => await Task.FromResult(new AnalyzedLayer(new RawLexeme[] { new(s.IndexOf('a'), s.Trim().Length, t) })), "");

        // Act
        var layer = await sut.FindLexemes(str);

        // Assert
        layer.Count.Should().Be(1);
        layer[0].Start.Should().Be(start);
        layer[0].Length.Should().Be(length);
    }
}
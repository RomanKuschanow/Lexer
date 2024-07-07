using FluentAssertions;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Moq;

namespace Lexer.Tests.Rules.RawResults;
public class RawLayerCreatorFixtures
{
    [Fact]
    public void GivenRawLexemesWithoutOverlap_WhenCreate_ThenLayerContainGivenLexemes()
    {
        // Arrange
        var lexemes = Enumerable.Range(0, 10).Select(i =>
        {
            var lexeme = new Mock<IRawLexeme>();
            lexeme.SetupGet(x => x.Start).Returns(i);
            lexeme.SetupGet(x => x.Length).Returns(1);
            return lexeme.Object;
        });

        RawLayerCreator sut = new();

        // Act
        var result = sut.Create(lexemes, Mock.Of<IRule>());

        // Assert
        result.RawLexemes.Should().BeEquivalentTo(lexemes);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GivenRawLexemesWithOverlap_WhenCreate_ThenLayerContainLexemesWithoutOverlap(bool selectShorter)
    {
        // Arrange
        IRule rule = Mock.Of<IRule>();

        /*
        ___ ___ __ 
         ____   ___
        */
        List<IRawLexeme> lexemes = new()
        {
            new RawLexeme(0, 3, rule, ""),
            new RawLexeme(1, 4, rule, ""),
            new RawLexeme(4, 3, rule, ""),
            new RawLexeme(8, 3, rule, ""),
            new RawLexeme(8, 2, rule, ""),
        };
        List<IRawLexeme> expectedLexemes = new()
        {
            new RawLexeme(0, 3, rule, ""),
            new RawLexeme(4, 3, rule, ""),
            new RawLexeme(8, selectShorter ? 2 : 3, rule, ""),
        };

        RawLayerCreator sut = new(selectShorter);

        // Act
        var result = sut.Create(lexemes, rule);

        // Assert
        result.RawLexemes.Should().BeEquivalentTo(expectedLexemes);
    }
}

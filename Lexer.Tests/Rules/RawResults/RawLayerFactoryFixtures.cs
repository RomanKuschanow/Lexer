using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;

namespace Lexer.Tests.Rules.RawResults;
public class RawLayerFactoryFixtures
{
    [Fact]
    public void GivenRawLayerCreator_WhenAddConcreteCreator_ThenRawLayerFactoryContainSpecifiedRawLayerCreator()
    {
        // Arrange
        var rawLayerCreatorMock = new Mock<IRawLayerCreator>();

        RawLayerFactory sut = new();

        // Act
        sut.AddConcreteCreator(rawLayerCreatorMock.Object);

        // Assert
        sut.RawLayerCreators.Should().Contain(rawLayerCreatorMock.Object);
    }

    [Fact]
    public void GivenRuleAndLexemes_WhenCreateRawLayer_ThenRawLayerContainSpecifiedLexemes()
    {
        // Arrange
        var lexemes = Enumerable.Range(0, 10).Select(i =>
        {
            var lexeme = new Mock<IRawLexeme>();
            lexeme.SetupGet(x => x.Start).Returns(i);
            lexeme.SetupGet(x => x.Length).Returns(1);
            return lexeme.Object;
        });

        RawLayerCreator rawLayerCreator = new();

        RawLayerFactory sut = new();
        sut.AddConcreteCreator(rawLayerCreator);

        // Act
        var result = sut.CreateRawLayer(new Mock<RuleBase>("", Mock.Of<IRuleSettings>()).Object, lexemes);

        // Assert
        result.RawLexemes.Should().BeEquivalentTo(lexemes);
    }

}

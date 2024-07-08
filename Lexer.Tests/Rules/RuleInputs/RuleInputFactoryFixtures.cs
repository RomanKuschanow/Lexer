using FluentAssertions;
using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RuleInputs;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;

namespace Lexer.Tests.Rules.RuleInputs;
public class RuleInputFactoryFixtures
{
    [Fact]
    public void GivenRuleInputCreator_WhenAddConcreteCreator_ThenRuleInputFactoryContainSpecifiedRuleInputCreator()
    {
        // Arrange
        var ruleInputCreatorMock = new Mock<IRuleInputCreator>();

        RuleInputFactory sut = new();

        // Act
        sut.AddConcreteCreator(ruleInputCreatorMock.Object);

        // Assert
        sut.RuleInputCreators.Should().Contain(ruleInputCreatorMock.Object);
    }

    [Fact]
    public void GivenRuleAndIntermediateDataCollection_WhenCreateInput_ThenRuleInputContainSpecifiedData()
    {
        // Arrange
        string str = "some text";

        var dataCollectionMock = new Mock<IIntermediateDataCollection>();
        dataCollectionMock.Setup(x => x.Get<InputTextIntermediateData>()).Returns(new InputTextIntermediateData(str));

        CommonRuleInputCreator inputCreator = new();

        RuleInputFactory sut = new();
        sut.AddConcreteCreator(inputCreator);

        // Act
        var result = sut.CreateInput(new Mock<RuleBase>("", Mock.Of<IRuleSettings>()).Object, dataCollectionMock.Object);

        // Assert
        result.Text.Should().Be(str);
    }
}

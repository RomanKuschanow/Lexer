using FluentAssertions;
using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Rules.RuleInputs;
using Moq;

namespace Lexer.Tests.Rules.RuleInputs;
public class CommonRuleInputCreatorFixtures
{
    [Fact]
    public void GivenIntermediateDataCollectionWithInputTextIntermediateData_WhenCreate_ThenRuleInputContainsSpecifiedText()
    {
        // Arrange
        string str = "some text";

        var dataCollectionMock = new Mock<IIntermediateDataCollection>();
        dataCollectionMock.Setup(x => x.Get<InputTextIntermediateData>()).Returns(new InputTextIntermediateData(str));

        CommonRuleInputCreator sut = new();

        // Act
        var ruleInput = sut.Create(dataCollectionMock.Object);

        // Assert
        ruleInput.Should().NotBeNull();
        ruleInput.Text.Should().Be(str);
    }
}

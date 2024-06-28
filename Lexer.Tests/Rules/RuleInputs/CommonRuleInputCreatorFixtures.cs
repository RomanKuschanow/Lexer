using FluentAssertions;
using Lexer.Analyzer.Interfaces;
using Lexer.Analyzer.IntermediateData;
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

        IntermediateDataCollection intermediateDataCollection = new();
        intermediateDataCollection.Add(new InputTextIntermediateData(str));
        CommonRuleInputCreator sut = new();

        // Act
        var commonRuleInput = sut.Create(intermediateDataCollection);

        // Assert
        commonRuleInput.Text.Should().Be(str);
    }
}

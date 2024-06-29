#nullable disable
using FluentAssertions;
using Lexer.Analyzer.Interfaces;
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;

namespace Lexer.Tests.Rules.RuleInputs;
public class DependedRuleInputCreatorFixtures
{
    [Fact]
    public void GivenIntermediateDataCollectionWithProcessedLayersIntermediateData_WhenCreate_ThenRuleInputContainsSpecifiedTextAndRawLayers()
    {
        // Arrange
        string str = "some text";
        var RuleMock = new Mock<IRule>();
        var RawLayerMock = new Mock<IRawLayer>();
        Dictionary<IRule, IRawLayer> layer = new() { { RuleMock.Object, RawLayerMock.Object } };

        var dataCollectionMock = new Mock<IIntermediateDataCollection>();
        dataCollectionMock.Setup(x => x.Get<InputTextIntermediateData>()).Returns(new InputTextIntermediateData(str));
        dataCollectionMock.Setup(x => x.Get<ProcessedLayersIntermediateData>()).Returns(new ProcessedLayersIntermediateData(layer));

        DependedRuleInputCreator sut = new();

        // Act
        var ruleInput = sut.Create(dataCollectionMock.Object) as IDependedRuleInput;

        // Assert
        ruleInput.Should().NotBeNull();
        ruleInput.Text.Should().Be(str);
        ruleInput.Dependencies.Should().BeEquivalentTo(layer);
    }
}

using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.IntermediateData.Interfaces;
using Lexer.Analyzer.Middleware;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Tests.Analyzer.Middleware;
public class AddLayerToDataCollectionFixtures
{
    [Fact]
    public void Given_When_Then()
    {
        // Arrange
        AddLayerToDataCollection sut = new();
        var dataCollectionMock = new Mock<IIntermediateDataCollection>();
        ProcessedLayersIntermediateData layerData = new();
        dataCollectionMock.Setup(x => x.Get<ProcessedLayersIntermediateData>()).Returns(layerData);
        var rule = Mock.Of<IRule>();
        var rawLayer = Mock.Of<IRawLayer>();

        // Act
        sut.Execute(rule, Mock.Of<IRuleInput>(), rawLayer, dataCollectionMock.Object);

        // Assert
        layerData.GetDataByKey(rule).Should().Be(rawLayer);
    }
}

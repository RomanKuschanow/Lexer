using FluentAssertions;
using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults.Interfaces;
using Moq;
using System.Reflection;

namespace Lexer.Tests.Analyzer.IntermediateData;
public class ProcessedLayersIntermediateDataFixtures
{
    [Fact]
    public void GivenDictionary_WhenCreateNewInstance_ThenProcessedLayersContainsSpecifiedDictionary()
    {
        // Arrange
        Dictionary<IRule, IRawLayer> dict = new() { { Mock.Of<IRule>(), Mock.Of<IRawLayer>() } };

        // Act
        ProcessedLayersIntermediateData sut = new(dict);

        // Assert
        typeof(ProcessedLayersIntermediateData).GetProperty("ProcessedLayers", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut).Should().BeEquivalentTo(dict);
    }

    [Fact]
    public void GivenDictionary_WhenSetData_ThenProcessedLayersPropertyHasSpecifiedValue()
    {
        // Arrange
        Dictionary<IRule, IRawLayer> dict = new() { { Mock.Of<IRule>(), Mock.Of<IRawLayer>() } };
        ProcessedLayersIntermediateData sut = new();

        // Act
        sut.SetData(dict);

        // Assert
        typeof(ProcessedLayersIntermediateData).GetProperty("ProcessedLayers", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut).Should().BeEquivalentTo(dict);
    }

    [Fact]
    public void GivenProcessedLayersIntermediateDataWithSpecifiedData_WhenGetData_ThenReturnSpecifiedDictionary()
    {
        // Arrange
        Dictionary<IRule, IRawLayer> dict = new() { { Mock.Of<IRule>(), Mock.Of<IRawLayer>() } };
        ProcessedLayersIntermediateData sut = new(dict);

        // Act
        sut.GetData()

        // Assert
        .Should().BeEquivalentTo(dict);
    }

    [Fact]
    public void GivenProcessedLayersIntermediateData_WhenSetDataByKey_ThenProcessedLayersPropertyHasAddedKeyValuePair()
    {
        // Arrange
        var rule = Mock.Of<IRule>();
        var layer = Mock.Of<IRawLayer>();
        Dictionary<IRule, IRawLayer> dict = new();
        ProcessedLayersIntermediateData sut = new();

        // Act
        sut.SetDataByKey(rule, layer);

        // Assert
        sut.GetData().Should().Contain(new KeyValuePair<IRule, IRawLayer>(rule, layer));
    }

    [Fact]
    public void GivenProcessedLayersIntermediateData_WhenGetDataByKey_ThenReturnSpecifiedValue()
    {
        // Arrange
        var rule = Mock.Of<IRule>();
        var layer = Mock.Of<IRawLayer>();
        Dictionary<IRule, IRawLayer> dict = new() { { rule, layer } };
        ProcessedLayersIntermediateData sut = new(dict);

        // Act
        sut.GetDataByKey(rule)

        // Assert
        .Should().Be(layer);
    }
}

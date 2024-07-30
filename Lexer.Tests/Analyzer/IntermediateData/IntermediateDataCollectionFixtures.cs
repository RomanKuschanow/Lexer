using Lexer.Analyzer.IntermediateData;
using Lexer.Analyzer.IntermediateData.Interfaces;
using System.Reflection;

namespace Lexer.Tests.Analyzer.IntermediateData;
public class IntermediateDataCollectionFixtures
{
    [Fact]
    public void GivenIntermediateData_WhenAdd_ThenIntermediateDataCollectionContainsSpecifiedData()
    {
        // Arrange
        var intermediateDataMock = Mock.Of<IIntermediateData>();
        IntermediateDataCollection sut = new();

        // Act
        sut.Add(intermediateDataMock);

        // Assert
        (typeof(IntermediateDataCollection).GetField("_dataDict", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut) as Dictionary<Type, IIntermediateData>)!.Values.Should().Contain(intermediateDataMock);
    }    
    
    [Fact]
    public void GivenIntermediateDataAndEmptyIntermediateDataCollection_WhenTryAdd_ThenReturnsTrueAndIntermediateDataCollectionContainsSpecifiedData()
    {
        // Arrange
        var intermediateDataMock = Mock.Of<IIntermediateData>();
        IntermediateDataCollection sut = new();

        // Act
        sut.TryAdd(intermediateDataMock)

        // Assert
        .Should().BeTrue();
        (typeof(IntermediateDataCollection).GetField("_dataDict", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut) as Dictionary<Type, IIntermediateData>)!.Values.Should().Contain(intermediateDataMock);
    }
    
    [Fact]
    public void GivenIntermediateDataAndIntermediateDataCollectionWithSpecifiedIntermediateData_WhenTryAdd_ThenReturnsFalse()
    {
        // Arrange
        var intermediateDataMock = Mock.Of<IIntermediateData>();
        IntermediateDataCollection sut = new();
        sut.Add(intermediateDataMock);

        // Act
        sut.TryAdd(intermediateDataMock)

        // Assert
        .Should().BeFalse();
    }

    [Fact]
    public void GivenIntermediateData_WhenGet_ThenReturnsSpecifiedData()
    {
        // Arrange
        InputTextIntermediateData data = new();
        IntermediateDataCollection sut = new();
        sut.Add(data);

        // Act
        sut.Get<InputTextIntermediateData>()

        // Assert
        .Should().Be(data);
    }

    [Fact]
    public void GivenIntermediateDataAndIntermediateDataCollectionWithSpecifiedIntermediateData_WhenRemove_ThenIntermediateDataCollectionNotContainsSpecifiedData()
    {
        // Arrange
        InputTextIntermediateData data = new();
        IntermediateDataCollection sut = new();
        sut.Add(data);

        // Act
        sut.Remove<InputTextIntermediateData>();

        // Assert
        (typeof(IntermediateDataCollection).GetField("_dataDict", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut) as Dictionary<Type, IIntermediateData>)!.Values.Should().NotContain(data);
    }
}

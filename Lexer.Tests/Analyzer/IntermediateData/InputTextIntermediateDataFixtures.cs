using FluentAssertions;
using Lexer.Analyzer.IntermediateData;
using System.Reflection;

namespace Lexer.Tests.Analyzer.IntermediateData;
public class InputTextIntermediateDataFixtures
{

    [Fact]
    public void GivenString_WhenCreatingNewInstance_ThenTextPropertyHasSpecifiedValue()
    {
        // Arrange
        string str = "test";

        // Act
        InputTextIntermediateData sut = new(str);

        // Assert
        typeof(InputTextIntermediateData).GetProperty("Text", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut).Should().Be(str);
    }

    [Fact]
    public void GivenString_WhenSetData_ThenTextPropertyHasSpecifiedValue()
    {
        // Arrange
        string str = "test";
        InputTextIntermediateData sut = new("");

        // Act
        sut.SetData(str);

        // Assert
        typeof(InputTextIntermediateData).GetProperty("Text", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(sut).Should().Be(str);
    }

    [Fact]
    public void GivenInputTextIntermediateDataWithSpecifiedData_WhenGetData_ThenReturnSpecifiedString()
    {
        // Arrange
        string str = "test";
        InputTextIntermediateData sut = new(str);

        // Act
        sut.GetData()

        // Assert
        .Should().Be(str);
    }
}

using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.Visitors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer.Tests.CommonRulesTests;
public class CustomRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> func = async (input, ct, rule) => await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>());
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("custom");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        var sut = new CustomRule(func, mockRuleSettings.Object);

        // Assert
        sut.Type.Should().Be("custom");
        sut.IsIgnored.Should().BeFalse();
        sut.IsOnlyForDependentRules.Should().BeFalse();
        sut.IsEnabled.Should().BeTrue();
        sut.Func.Should().Be(func);
    }

    [Fact]
    public void GivenNullFunc_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("custom");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        // Act
        Action act = () => new CustomRule(null, mockRuleSettings.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        var expectedLexemes = new List<RawLexeme>
        {
            new RawLexeme(0, 4, null),
            new RawLexeme(5, 2, null)
        };

        Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> func = async (input, ct, rule) =>
        {
            return await AnalyzedLayer.FromIEnumerable(expectedLexemes);
        };

        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("custom");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new CustomRule(func, mockRuleSettings.Object);

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("sample text");

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedLexemes);
    }

    [Fact]
    public void GivenVisitorAndVisitorInput_WhenAccept_ThenCallsVisitorRule()
    {
        // Arrange
        Func<IRuleInput, CancellationToken, IRule, Task<AnalyzedLayer>> func = async (input, ct, rule) => await AnalyzedLayer.FromIEnumerable(new List<RawLexeme>());
        var mockRuleSettings = new Mock<IRuleSettings>();
        mockRuleSettings.SetupGet(x => x.Type).Returns("custom");
        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

        var sut = new CustomRule(func, mockRuleSettings.Object);

        var mockVisitor = new Mock<IVisitor>();
        var visitorInput = new VisitorInput("sample text", new Dictionary<IRule, AnalyzedLayer>());

        // Act
        sut.Accept(mockVisitor.Object, visitorInput);

        // Assert
        mockVisitor.Verify(v => v.Rule(visitorInput), Times.Once);
    }
}

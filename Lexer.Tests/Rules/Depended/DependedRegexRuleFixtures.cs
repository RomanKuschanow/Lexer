using FluentAssertions;
using Lexer.Rules.Depended;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RawResults.Interfaces;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Lexer.Tests.Rules.Depended;
public class DependedRegexRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        string pattern = @"<rule1>";

        // Act
        var sut = new DependedRegexRule(pattern, "depended");

        // Assert
        sut.Pattern.Should().Be(pattern);
        sut.RegexOptions.Should().Be(RegexOptions.None);
        sut.RuleOptions.Should().Be(DependedRuleOptions.None);
        sut.Type.Should().Be("depended");
        sut.IsIgnored.Should().BeFalse();
        sut.IsOnlyForDependentRules.Should().BeFalse();
        sut.IsEnabled.Should().BeTrue();
    }

    [Fact]
    public void GivenNullPattern_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Arrange
        // Act
        Action act = () => _ = new DependedRegexRule(null, "depended");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string type = "depended";
        string pattern = @"<rule1>\s<rule2>";
        var sut = new DependedRegexRule(pattern, type);

        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var analyzedLayerMock1 = new Mock<IRawLayer>();
        analyzedLayerMock1.Setup(x => x.RawLexemes).Returns(new List<RawLexeme>
        {
            new(0, 4, mockRule1.Object, type), // test
            new(9, 3, mockRule2.Object, type) // bar
        });
        var analyzedLayerMock2 = new Mock<IRawLayer>();
        analyzedLayerMock2.Setup(x => x.RawLexemes).Returns(new List<RawLexeme>
        {
            new(5, 3, mockRule1.Object, type),  // foo
            new(13, 3, mockRule2.Object, type) // baz
        });

        sut.AddDependency(mockRule1.Object, "rule1");
        sut.AddDependency(mockRule2.Object, "rule2");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo bar baz");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, IRawLayer>
        {
            { mockRule1.Object, analyzedLayerMock1.Object },
            { mockRule2.Object, analyzedLayerMock2.Object }
        }.ToImmutableDictionary());

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 8);  // test foo
        result.Should().Contain(lexeme => lexeme.Start == 9 && lexeme.Length == 7);  // bar baz
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemesWithAlwaysSameData_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string type = "depended";
        string pattern = @"<rule1>\s<rule2>\s<rule1>";
        var sut = new DependedRegexRule(pattern, type, new(DependedRuleOptions.AlwaysSameData));

        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var analyzedLayerMock1 = new Mock<IRawLayer>();
        analyzedLayerMock1.Setup(x => x.RawLexemes).Returns(new List<RawLexeme>
        {
            new(0, 4, mockRule1.Object, type), // test
            new(23, 5, mockRule1.Object, type), // _test
        });
        var analyzedLayerMock2 = new Mock<IRawLayer>();
        analyzedLayerMock2.Setup(x => x.RawLexemes).Returns(new List<RawLexeme>
        {
            new(5, 3, mockRule1.Object, type),  // foo
            new(19, 3, mockRule1.Object, type)  // foo
        });

        sut.AddDependency(mockRule1.Object, "rule1");
        sut.AddDependency(mockRule2.Object, "rule2");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo test test foo _test");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, IRawLayer>
        {
            { mockRule1.Object, analyzedLayerMock1.Object },
            { mockRule2.Object, analyzedLayerMock2.Object }
        }.ToImmutableDictionary());

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 13);  // test foo test
    }

    [Fact]
    public void GivenValidInput_WhenFindingLexemesWithoutAlwaysSameData_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string type = "depended";
        string pattern = @"<rule1>\s<rule1>";
        var sut = new DependedRegexRule(pattern, type);

        var mockRule1 = new Mock<IRule>();
        var analyzedLayerMock = new Mock<IRawLayer>();
        analyzedLayerMock.Setup(x => x.RawLexemes).Returns(new List<RawLexeme>
        {
            new(0, 4, mockRule1.Object, type), // test
            new(5, 3, mockRule1.Object, type)  // foo
        });

        sut.AddDependency(mockRule1.Object, "rule1");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo test foo");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, IRawLayer>
        {
            { mockRule1.Object, analyzedLayerMock.Object }
        }.ToImmutableDictionary());

        // Act
        var result = sut.FindLexemes(mockInput.Object);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 8);  // test foo
        result.Should().Contain(lexeme => lexeme.Start == 5 && lexeme.Length == 8);  // foo test
        result.Should().Contain(lexeme => lexeme.Start == 9 && lexeme.Length == 8);  // test foo
    }

    [Fact]
    public void GivenInvalidInput_WhenFindingLexemes_ThenThrowsArgumentException()
    {
        // Arrange
        string pattern = @"<rule1>";
        var sut = new DependedRegexRule(pattern, "depended");

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test");

        // Act
        Action act = () => _ = sut.FindLexemes(mockInput.Object);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}


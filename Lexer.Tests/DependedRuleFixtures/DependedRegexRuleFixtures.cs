using FluentAssertions;
using Lexer.Rules.Depended;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Lexer.Tests.DependedRuleFixtures;
public class DependedRegexRuleFixtures
{
    [Fact]
    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
    {
        // Arrange
        string pattern = @"<rule1>";
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.None);

        // Act
        var sut = new DependedRegexRule(pattern, ruleSettings);

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
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.None);

        // Act
        Action act = () => new DependedRegexRule(null, ruleSettings);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string pattern = @"<rule1>\s<rule2>";
        var ruleSettings = new DependedRegexRuleSettings("depended");
        var sut = new DependedRegexRule(pattern, ruleSettings);

        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var analyzedLayer1 = await RawLayer.FromIEnumerable(new List<RawLexeme>
        {
            new RawLexeme(0, 4, mockRule1.Object), // test
            new RawLexeme(9, 3, mockRule2.Object) // bar
        });
        var analyzedLayer2 = await RawLayer.FromIEnumerable(new List<RawLexeme>
        {
            new RawLexeme(5, 3, mockRule1.Object),  // foo
            new RawLexeme(13, 3, mockRule2.Object) // baz
        });

        sut.AddDependency(mockRule1.Object, "rule1");
        sut.AddDependency(mockRule2.Object, "rule2");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo bar baz");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, RawLayer>
        {
            { mockRule1.Object, analyzedLayer1 },
            { mockRule2.Object, analyzedLayer2 }
        }.ToImmutableDictionary());

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 8);  // test foo
        result.Should().Contain(lexeme => lexeme.Start == 9 && lexeme.Length == 7);  // bar baz
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemesWithAlwaysSameData_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string pattern = @"<rule1>\s<rule2>\s<rule1>";
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.AlwaysSameData);
        var sut = new DependedRegexRule(pattern, ruleSettings);

        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var analyzedLayer1 = await RawLayer.FromIEnumerable(new List<RawLexeme>
        {
            new RawLexeme(0, 4, mockRule1.Object), // test
        });
        var analyzedLayer2 = await RawLayer.FromIEnumerable(new List<RawLexeme>
        {
            new RawLexeme(5, 3, mockRule1.Object),  // foo
            new RawLexeme(14, 3, mockRule1.Object)  // foo
        });

        sut.AddDependency(mockRule1.Object, "rule1");
        sut.AddDependency(mockRule2.Object, "rule2");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo test foo");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, RawLayer>
        {
            { mockRule1.Object, analyzedLayer1 },
            { mockRule2.Object, analyzedLayer2 }
        }.ToImmutableDictionary());

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 13);  // test foo test
    }

    [Fact]
    public async Task GivenValidInput_WhenFindingLexemesWithoutAlwaysSameData_ThenReturnsExpectedLexemes()
    {
        // Arrange
        string pattern = @"<rule1>\s<rule1>";
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.None);
        var sut = new DependedRegexRule(pattern, ruleSettings);

        var mockRule1 = new Mock<IRule>();
        var analyzedLayer1 = await RawLayer.FromIEnumerable(new List<RawLexeme>
        {
            new RawLexeme(0, 4, mockRule1.Object), // test
            new RawLexeme(5, 3, mockRule1.Object)  // foo
        });

        sut.AddDependency(mockRule1.Object, "rule1");

        var mockInput = new Mock<IDependedRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test foo test foo");
        mockInput.SetupGet(x => x.Dependencies).Returns(new Dictionary<IRule, RawLayer>
        {
            { mockRule1.Object, analyzedLayer1 }
        }.ToImmutableDictionary());

        // Act
        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(lexeme => lexeme.Start == 0 && lexeme.Length == 8);  // test foo
        result.Should().Contain(lexeme => lexeme.Start == 9 && lexeme.Length == 8);  // test foo
    }

    [Fact]
    public async Task GivenInvalidInput_WhenFindingLexemes_ThenThrowsArgumentException()
    {
        // Arrange
        string pattern = @"<rule1>";
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.None);
        var sut = new DependedRegexRule(pattern, ruleSettings);

        var mockInput = new Mock<IRuleInput>();
        mockInput.SetupGet(x => x.Text).Returns("test");

        // Act
        Func<Task> act = async () => await sut.FindLexemes(mockInput.Object, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void GivenVisitorAndVisitorInput_WhenAccept_ThenCallsVisitorDependencyRule()
    {
        // Arrange
        string pattern = @"<rule1>";
        var ruleSettings = new DependedRegexRuleSettings("depended", DependedRuleOptions.None);
        var sut = new DependedRegexRule(pattern, ruleSettings);

        var mockVisitor = new Mock<IRuleVisitor>();
        var visitorInput = new VisitorInput("sample text", new Dictionary<IRule, RawLayer>());

        // Act
        sut.Accept(mockVisitor.Object, visitorInput);

        // Assert
        mockVisitor.Verify(v => v.DependencyRule(visitorInput, sut), Times.Once);
    }
}


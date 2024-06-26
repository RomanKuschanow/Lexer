using FluentAssertions;
using Lexer.Rules;
using Lexer.Rules.Interfaces;
using Moq;
using System.Collections.Immutable;

namespace Lexer.Tests;
public class RuleSetFixtures
{
    [Fact]
    public void GivenEmptyConstructor_WhenInstantiated_ThenCountIsZero()
    {
        // Arrange
        var ruleSet = new RuleSet();

        // Act
        var count = ruleSet.Count;

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void GivenRules_WhenInstantiated_ThenCountMatches()
    {
        // Arrange
        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var rules = new List<IRule> { mockRule1.Object, mockRule2.Object };
        var ruleSet = new RuleSet(rules);

        // Act
        var count = ruleSet.Count;

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public void GivenRuleSet_WhenAddingRule_ThenCountIncreases()
    {
        // Arrange
        var ruleSet = new RuleSet();
        var mockRule = new Mock<IRule>();

        // Act
        ruleSet.Add(mockRule.Object);
        var count = ruleSet.Count;

        // Assert
        count.Should().Be(1);
    }

    [Fact]
    public void GivenRuleSet_WhenRemovingRule_ThenCountDecreases()
    {
        // Arrange
        var mockRule1 = new Mock<IRule>();
        var mockRule2 = new Mock<IRule>();
        var rules = new List<IRule> { mockRule1.Object, mockRule2.Object };
        var ruleSet = new RuleSet(rules);

        // Act
        ruleSet.Remove(mockRule1.Object);
        var count = ruleSet.Count;

        // Assert
        count.Should().Be(1);
    }

    [Fact]
    public async Task GivenDependedRule_WhenPrepareRules_ThenDependenciesAreChecked()
    {
        // Arrange
        var mockDependedRule = new Mock<IDependedRule>();
        var mockDependency = new Mock<IRule>();
        mockDependedRule.Setup(r => r.IsEnabled).Returns(true);
        mockDependedRule.Setup(r => r.Dependencies).Returns(new Dictionary<IRule, string[]> { { mockDependency.Object, new string[0] } }.ToImmutableDictionary());

        var ruleSet = new RuleSet(new List<IRule> { mockDependedRule.Object, mockDependency.Object });

        // Act
        await ruleSet.PrepareRules();

        // Assert
        mockDependedRule.Verify(r => r.RemoveDependency(mockDependency.Object), Times.Once);
    }

    [Fact]
    public void GivenDependedRule_WhenAddDependency_ThenDependencyIsAdded()
    {
        // Arrange
        var mockDependedRule = new Mock<IDependedRule>();
        var mockDependency = new Mock<IRule>();
        mockDependedRule.Setup(r => r.IsEnabled).Returns(true);
        mockDependency.Setup(r => r.IsEnabled).Returns(true);

        var ruleSet = new RuleSet(new List<IRule> { mockDependedRule.Object, mockDependency.Object });

        // Act
        ruleSet.AddDependency(mockDependedRule.Object, mockDependency.Object);

        // Assert
        mockDependedRule.Verify(r => r.AddDependency(mockDependency.Object), Times.Once);
    }

    [Fact]
    public void GivenDependedRule_WhenRemoveDependency_ThenDependencyIsRemoved()
    {
        // Arrange
        var mockDependedRule = new Mock<IDependedRule>();
        var mockDependency = new Mock<IRule>();
        mockDependedRule.Setup(r => r.IsEnabled).Returns(true);
        mockDependedRule.Setup(r => r.Dependencies).Returns(new Dictionary<IRule, string[]> { { mockDependency.Object, new string[0] } }.ToImmutableDictionary());

        var ruleSet = new RuleSet(new List<IRule> { mockDependedRule.Object, mockDependency.Object });

        // Act
        ruleSet.RemoveDependency(mockDependedRule.Object, mockDependency.Object);

        // Assert
        mockDependedRule.Verify(r => r.RemoveDependency(mockDependency.Object), Times.Once);
    }

    [Fact]
    public void GivenDependedRule_WhenClearDependencies_ThenDependenciesAreCleared()
    {
        // Arrange
        var mockDependedRule = new Mock<IDependedRule>();
        mockDependedRule.Setup(r => r.IsEnabled).Returns(true);
        mockDependedRule.Setup(r => r.Dependencies).Returns(new Dictionary<IRule, string[]> { { new Mock<IRule>().Object, new string[0] } }.ToImmutableDictionary());

        var ruleSet = new RuleSet(new List<IRule> { mockDependedRule.Object });

        // Act
        ruleSet.ClearDependencies(mockDependedRule.Object);

        // Assert
        mockDependedRule.Verify(r => r.ClearDependencies(), Times.Once);
    }

    [Fact]
    public void GivenRuleSet_WhenGettingEnumerator_ThenReturnsEnabledRules()
    {
        // Arrange
        var mockRule1 = new Mock<IRule>();
        mockRule1.Setup(r => r.IsEnabled).Returns(true);
        var mockRule2 = new Mock<IRule>();
        mockRule2.Setup(r => r.IsEnabled).Returns(false);

        var ruleSet = new RuleSet(new List<IRule> { mockRule1.Object, mockRule2.Object });

        // Act
        var enabledRules = ruleSet.ToList();

        // Assert
        enabledRules.Should().HaveCount(1);
        enabledRules.First().Should().Be(mockRule1.Object);
    }
}

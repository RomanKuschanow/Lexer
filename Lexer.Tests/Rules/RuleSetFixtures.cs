using Lexer.Rules;
using Lexer.Rules.Interfaces;

namespace Lexer.Tests.Rules;
public class RuleSetFixtures
{
    [Fact]
    public void GivenRule_WhenAdd_ThenRuleSetContainSpecifiedRule()
    {
        // Arrange
        var ruleMock = new Mock<IRule>();
        ruleMock.SetupGet(x => x.IsEnabled).Returns(true);

        RuleSet sut = new();

        // Act
        sut.Add(ruleMock.Object);

        // Assert
        sut.Rules.Should().Contain(ruleMock.Object);
    }

    [Fact]
    public void GivenRules_WhenAddRange_ThenRuleSetContainSpecifiedRules()
    {
        // Arrange
        var ruleMock1 = new Mock<IRule>();
        var ruleMock2 = new Mock<IRule>();
        ruleMock1.SetupGet(x => x.IsEnabled).Returns(true);
        ruleMock2.SetupGet(x => x.IsEnabled).Returns(true);

        var rules = new IRule[] { ruleMock1.Object, ruleMock2.Object };

        RuleSet sut = new();

        // Act
        sut.AddRange(rules);

        // Assert
        sut.Rules.Should().Contain(rules);
    }

    [Fact]
    public void GivenRule_WhenInsertBefore_ThenRuleSetContainSpecifiedRuleOnSpecifiedPlace()
    {
        // Arrange
        var ruleMock1 = new Mock<IRule>();
        var ruleMock2 = new Mock<IRule>();
        ruleMock1.SetupGet(x => x.IsEnabled).Returns(true);
        ruleMock2.SetupGet(x => x.IsEnabled).Returns(true);

        RuleSet sut = new();
        sut.Add(ruleMock1.Object);

        // Act
        sut.InsertBefore(ruleMock1.Object, ruleMock2.Object);

        // Assert
        sut.Rules.Should().Contain(ruleMock2.Object);
        sut.Rules.Should().ContainInOrder(ruleMock2.Object, ruleMock1.Object);
    }

    [Fact]
    public void GivenRuleSet_WhenRemove_ThenRuleSetNotContainRemovedRule()
    {
        // Arrange
        var ruleMock = new Mock<IRule>();
        ruleMock.SetupGet(x => x.IsEnabled).Returns(true);

        RuleSet sut = new();
        sut.Add(ruleMock.Object);

        // Act
        sut.Remove(ruleMock.Object);

        // Assert
        sut.Rules.Should().NotContain(ruleMock.Object);
    }

    [Fact]
    public void GivenRuleSet_WhenRemoveRange_ThenRuleSetNotContainRemovedRules()
    {
        // Arrange
        var ruleMock1 = new Mock<IRule>();
        var ruleMock2 = new Mock<IRule>();
        ruleMock1.SetupGet(x => x.IsEnabled).Returns(true);
        ruleMock2.SetupGet(x => x.IsEnabled).Returns(true);

        var rules = new IRule[] { ruleMock1.Object, ruleMock2.Object };

        RuleSet sut = new();
        sut.AddRange(rules);

        // Act
        sut.RemoveRange(rules);

        // Assert
        sut.Rules.Should().NotContain(rules);
    }

    [Fact]
    public void GivenRuleSet_WhenClear_ThenRuleSetNotContainAnyRules()
    {
        // Arrange
        var ruleMock = new Mock<IRule>();
        ruleMock.SetupGet(x => x.IsEnabled).Returns(true);

        RuleSet sut = new();
        sut.Add(ruleMock.Object);

        // Act
        sut.Clear();

        // Assert
        sut.Rules.Should().BeEmpty();
    }
}

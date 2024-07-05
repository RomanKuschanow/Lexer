using FluentAssertions;
using Lexer.Analyzer.Middleware;
using Lexer.Analyzer.Middleware.Interface;
using Lexer.Exceptions;
using Lexer.Rules.Depended;
using Lexer.Rules.Interfaces;
using Moq;

namespace Lexer.Tests.Analyzer.Middleware;
public class MiddlewareCollectionFixtures
{
    [Fact]
    public void GivenMiddleware_WhenAdd_ThenMiddlewareCollectionContainsSpecifiedMiddleware()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();

        // Act
        sut.Add(middleware);

        // Assert
        sut.Middleware.Should().Contain(middleware);
    }

    [Fact]
    public void GivenMiddleware_WhenAddAndMiddlewareCollectionContainsSpecifiedMiddleware_ThenThrowKeyException()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();
        sut.Add(middleware);

        // Act
        new Action(() => sut.Add(middleware))
        // Assert
        .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenMiddleware_WhenTryAddAndCollectionNotContainsSpecifiedMiddleware_ThenReturnsTrue()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();

        // Act
        sut.TryAdd(middleware)
        // Assert
        .Should().BeTrue();
    }

    [Fact]
    public void GivenMiddleware_WhenTryAddAndCollectionContainsSpecifiedMiddleware_ThenReturnsFalse()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();
        sut.Add(middleware);

        // Act
        sut.TryAdd(middleware)
        // Assert
        .Should().BeFalse();
    }

    [Fact]
    public void GivenMiddleware_WhenGetAndCollectionContainsSpecifiedMiddleware_ThenReturnSpecifiedMiddleware()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();
        sut.Add(middleware);

        // Act
        var result = sut.Get(middleware.GetType());

        // Assert
        result.Should().Be(middleware);
    }

    [Fact]
    public void GivenMiddleware_WhenRemove_ThenMiddlewareCollectionNotContainSpecifiedMiddleware()
    {
        // Arrange
        var middleware = Mock.Of<IMiddleware>();

        MiddlewareCollection sut = new();
        sut.Add(middleware);

        // Act
        sut.Remove(middleware.GetType())
        // Assert
        .Should().BeTrue();
    }

    [Fact]
    public void GivenRuleAndMiddleware_WhenGetMiddlewareByRule_ThenReturnSpecifiedMiddleware()
    {
        // Arrange
        var middleware = new AddLayerToDataCollection();

        MiddlewareCollection sut = new();
        sut.Add(middleware);

        // Act
        var result = sut.GetMiddlewareByRule(new Mock<DependedRuleBase>("", Mock.Of<IRuleSettings>()).Object);

        // Assert
        result.Should().Contain(middleware);
    }

    [Fact]
    public void GivenRuleWithNecessaryMiddleware_WhenGetMiddlewareByRule_ThenThrowNecessaryMiddlewareNotFoundException()
    {
        // Arrange
        MiddlewareCollection sut = new();

        // Act
        new Action(() => sut.GetMiddlewareByRule(new Mock<DependedRuleBase>("", Mock.Of<IRuleSettings>()).Object).ToList())
        // Assert
        .Should().Throw<NecessaryMiddlewareNotFoundException>();
    }
}

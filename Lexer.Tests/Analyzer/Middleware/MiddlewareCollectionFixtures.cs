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
    public void GivenTypeAndMiddleware_WhenAddIsCalled_ThenAddsMiddleware()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();
        var ruleType = typeof(Mock<IRule>);

        // Act
        collection.Add(ruleType, middlewareMock.Object);

        // Assert
        collection.Middleware[ruleType].Should().Contain(middlewareMock.Object);
    }

    [Fact]
    public void GivenTypeAndNullMiddleware_WhenAddIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var ruleType = typeof(Mock<IRule>);

        // Act
        Action act = () => collection.Add(ruleType, null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullTypeAndMiddleware_WhenAddIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();

        // Act
        Action act = () => collection.Add(null, middlewareMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenMiddleware_WhenGenericAddIsCalled_ThenAddsMiddleware()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();

        // Act
        collection.Add<IRule>(middlewareMock.Object);

        // Assert
        collection.Middleware[typeof(IRule)].Should().Contain(middlewareMock.Object);
    }

    [Fact]
    public void GivenNullMiddleware_WhenGenericAddIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        var collection = new MiddlewareCollection();

        // Act
        Action act = () => collection.Add<IRule>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenTypeAndMiddleware_WhenRemoveIsCalled_ThenRemovesMiddleware()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();
        var ruleType = typeof(IRule);
        collection.Add(ruleType, middlewareMock.Object);

        // Act
        var result = collection.Remove(ruleType);

        // Assert
        result.Should().BeTrue();
        collection.Middleware.Should().NotContainKey(ruleType);
    }

    [Fact]
    public void GivenGenericTypeAndMiddleware_WhenRemoveIsCalled_ThenRemovesMiddleware()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();
        collection.Add<IRule>(middlewareMock.Object);

        // Act
        var result = collection.Remove<IRule>();

        // Assert
        result.Should().BeTrue();
        collection.Middleware.Should().NotContainKey(typeof(IRule));
    }

    [Fact]
    public void GivenRule_WhenGetIsCalled_ThenReturnsMiddleware()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var middlewareMock = new Mock<IMiddleware>();
        var ruleMock = new Mock<IRule>();
        collection.Add(ruleMock.Object.GetType(), middlewareMock.Object);

        // Act
        var result = collection.Get(ruleMock.Object);

        // Assert
        result.Should().Contain(middlewareMock.Object);
    }

    [Fact]
    public void GivenRule_WhenGetIsCalled_ThenThrowsKeyNotFoundException()
    {
        // Arrange
        var collection = new MiddlewareCollection();
        var ruleMock = new Mock<IRule>();

        // Act
        Action act = () => collection.Get(ruleMock.Object);

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }
}

using FluentAssertions;
using Lexer.Rules.Common;
using Lexer.Rules.Interfaces;
using Lexer.Rules.RawResults;
using Lexer.Rules.RuleInputs.Interfaces;
using Moq;

namespace Lexer.Tests.Rules.Common;
//public class WordsRuleFixtures
//{
//    [Fact]
//    public void GivenValidInputs_WhenCreatingInstance_ThenPropertiesAreSetCorrectly()
//    {
//        // Arrange
//        var words = new List<string> { "word1", "word2" };
//        var mockRuleSettings = new Mock<IRuleSettings>();
//        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
//        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

//        // Act
//        var sut = new WordsRule(words, mockRuleSettings.Object);

//        // Assert
//        sut.Type.Should().Be("word");
//        sut.IsIgnored.Should().BeFalse();
//        sut.IsOnlyForDependentRules.Should().BeFalse();
//        sut.IsEnabled.Should().BeTrue();
//        sut.Words.Should().BeEquivalentTo(words);
//    }

//    [Fact]
//    public void GivenNullWords_WhenCreatingInstance_ThenThrowsArgumentNullException()
//    {
//        // Arrange
//        var mockRuleSettings = new Mock<IRuleSettings>();
//        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
//        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

//        // Act
//        Action act = () => new WordsRule(null, mockRuleSettings.Object);

//        // Assert
//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public async Task GivenValidInput_WhenFindingLexemes_ThenReturnsExpectedLexemes()
//    {
//        // Arrange
//        var words = new List<string> { "test", "sample" };
//        var mockRuleSettings = new Mock<IRuleSettings>();
//        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
//        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

//        var sut = new WordsRule(words, mockRuleSettings.Object);

//        var mockInput = new Mock<IRuleInput>();
//        mockInput.SetupGet(x => x.Text).Returns("This is a test sample.");

//        // Act
//        var result = await sut.FindLexemes(mockInput.Object, CancellationToken.None);

//        // Assert
//        result.Should().HaveCount(2);
//        result.Should().Contain(lexeme => lexeme.Start == 10 && lexeme.Length == 4); // test
//        result.Should().Contain(lexeme => lexeme.Start == 15 && lexeme.Length == 6); // sample
//    }

//    [Fact]
//    public void GivenVisitorAndVisitorInput_WhenAccept_ThenCallsVisitorRule()
//    {
//        // Arrange
//        var words = new List<string> { "word1", "word2" };
//        var mockRuleSettings = new Mock<IRuleSettings>();
//        mockRuleSettings.SetupGet(x => x.Type).Returns("word");
//        mockRuleSettings.SetupGet(x => x.IsIgnored).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsOnlyForDependentRules).Returns(false);
//        mockRuleSettings.SetupGet(x => x.IsEnabled).Returns(true);

//        var sut = new WordsRule(words, mockRuleSettings.Object);

//        var mockVisitor = new Mock<IRuleVisitor>();
//        var visitorInput = new VisitorInput("sample text", new Dictionary<IRule, RawLayer>());

//        // Act
//        sut.Accept(mockVisitor.Object, visitorInput);

//        // Assert
//        mockVisitor.Verify(v => v.Rule(visitorInput), Times.Once);
//    }
//}

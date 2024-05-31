using FluentAssertions;
using Lexer.Analyzer;
using Lexer.Rules;

namespace Lexer.Tests;
public class AnalyzerFixtures
{
    [Theory]
    [InlineData("""
        public class Program
        {
            private void Main()
            {
                Console.WriteLine(1234);
            }
        }
        """, 19)]
    [InlineData("""
        private void _func1()
        {
            Console.WriteLine(12+34);
        }
        """, 16)]
    private async Task GivenCorrectCode_WhenAnalyze_ThenResultIsCorrectAndErrorsCountIsZeroAndLexemesCountEqualToGivenCount(string str, int count)
    {
        // Arrange
        RuleSet rules = new(new List<IRule>() {
            new WordsRule(new[] {"public", "private", "class", "void" }, "Keyword"),
            new RegexRule(new(@"\b[_A-Za-z][_A-Za-z0-9]*\b"), "Name"),
            new RegexRule(new(@"\b\d+\b"), "IntNumber"),
            new RegexRule(new(@"[\(\)\{\}\.;]"), "Separator"),
            new RegexRule(new(@"[-+/*=]"), "Operator"),
            new RegexRule(new(@"\s+"), "Whitespace", isIgnored: true)
        });
        var sut = new LexemeAnalyzer(rules);

        // Act
        var result = await sut.Analyze(str);

        // Assert
        result.IsCorrect.Should().BeTrue();
        result.Errors.Count.Should().Be(0);
        result.Lexemes.Count.Should().Be(count);
    }
    
    [Theory]
    [InlineData("""
        public class Program : IEnumerable<string>
        {
            private void Main(string[] args)
            {
                Console.WriteLine(1234);
            }
        }
        """, 4)]
    [InlineData("""
        private Task<string> _func1()
        {
            return "12, 5";
        }
        """, 5)]
    private async Task GivenIncorrectCode_WhenAnalyze_ThenResultIsNotCorrectAndErrorsCountEqualToGivenCount(string str, int count)
    {
        // Arrange
        RuleSet rules = new(new List<IRule>() {
            new WordsRule(new[] {"public", "private", "class", "void" }, "Keyword"),
            new RegexRule(new(@"\b[_A-Za-z][_A-Za-z0-9]*\b"), "Name"),
            new RegexRule(new(@"\b\d+\b"), "IntNumber"),
            new RegexRule(new(@"[\(\)\{\}\.;]"), "Separator"),
            new RegexRule(new(@"[-+/*=]"), "Operator"),
            new RegexRule(new(@"\s+"), "Whitespace", isIgnored: true)
        });
        var sut = new LexemeAnalyzer(rules);

        // Act
        var result = await sut.Analyze(str);

        // Assert
        result.IsCorrect.Should().BeFalse();
        result.Errors.Count.Should().Be(count);
    }
}

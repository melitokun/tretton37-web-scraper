using System.Linq;
using Xunit;

namespace Tretton37WebScraper.Tests;

public class HtmlLinkFinderTests
{
    [Fact]
    public void Should_Return_One_Html_Link()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = "<a href=\"/something\">";

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Single(result);
    }
    
    [Fact]
    public void Should_Return_One_Html_Link_With_Path_That_Matches_With_Given_Link()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = "<a href=\"/something\">";

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal("/something", result.First().Path);
    }
    
    [Fact]
    public void Should_Not_Return_Anything_If_Link_Does_Not_Start_With_Slash()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = "<a href=\"www.google.com/something\">";

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void Should_Return_Empty_If_Content_Is_Empty()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = string.Empty;

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Empty(result);
    }
    
    [Theory]
    [InlineData("<img src=\"/assets/i/_tretton37_slogan_white.svg\">", 0)]
    [InlineData("<html><link rel=\"prefetch\" href=\"/assets/i/join.jpg\"><link rel=\"prefetch\" href=\"/assets/i/contact.jpg\"><link rel=\"prefetch\" href=\"/assets/i/covid-19.jpg\"></html>", 3)]
    [InlineData("<html><a class=\"typeform-share button\" href=\"https://form.typeform.com/to/wmJczaqk\"></a><a href=\"https://github.com/tretton37\" target=\"_blank\"class=\"icon-github big\"></a> <a href=\"https://instagram.com/tretton37ab\" target=\"_blank\" class=\"icon-instagram big\"></a></html>", 0)]
    public void Should_FindAll_Result(string content, int numberOfLinks)
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal(numberOfLinks, result.Count());
    }

    [Fact]
    public void Should_Remove_Hash_From_The_Path()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = "<a href=\"/something#test-test\">";

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal("/something", result.First().Path);
    }
    
    [Fact]
    public void Should_Remove_Query_Strings_From_The_Path()
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();
        string content = "<a href=\"/something?test=test&a=b\">";

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal("/something", result.First().Path);
    }
    
    [Theory]
    [InlineData("<a href=\"/something#test-test?test=test&a=b\">", "/something")]
    [InlineData("<a href=\"/something?test=test&a=b#abc\">", "/something")]
    [InlineData("<a href=\"/test?test=test&a=b#something\">", "/test")]
    [InlineData("<a href=\"/test#something?test=test&a=b\">", "/test")]
    public void Should_Remove_Both_Hash_And_Query_Strings_From_The_Path(string content, string expected)
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal(expected, result.First().Path);
    }

    [Theory]
    [InlineData("<a href=\"/something.css\">", true)]
    [InlineData("<a href=\"/test/something/picture.png\">", true)]
    [InlineData("<a href=\"/test/test/something\">", false)]
    public void Should_Return_If_Link_Is_For_A_File(string content, bool isFile)
    {
        // Arrange
        var htmlLinkFinder = new HtmlLinkFinder();

        // Act
        var result = htmlLinkFinder.FindAll(content, string.Empty);

        // Assert
        Assert.Equal(isFile, result.First().IsFile);
    }
}
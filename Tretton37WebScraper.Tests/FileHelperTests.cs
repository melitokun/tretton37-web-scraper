using System.Threading.Tasks;
using Tretton37WebScraper.Abstractions;
using Xunit;

namespace Tretton37WebScraper.Tests;

public class FileHelperTests
{
    [Theory]
    [InlineData("file.txt", "file.txt")]
    [InlineData("/root/path/../file.txt", "/root/path/../file.txt")]
    [InlineData("/root/path/image.png", "/root/path/image.png")]
    public void Should_Return_File_Path_As_Is(string path, string expected)
    {
        // Arrange
        var htmlLink = new HtmlLink
        {
            Path = path,
            IsFile = true
        };
        
        // Act
        var result = FileHelper.GetFilePath(htmlLink);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("", "/index.html")]
    [InlineData("/root/path/../index", "/root/path/../index/index.html")]
    [InlineData("/root/path", "/root/path/index.html")]
    public void Should_Return_File_Path_By_Adding_Index_Html(string path, string expected)
    {
        // Arrange
        var htmlLink = new HtmlLink
        {
            Path = path,
            IsFile = false
        };
        
        // Act
        var result = FileHelper.GetFilePath(htmlLink);
        
        // Assert
        Assert.Equal(expected, result);
    }
}
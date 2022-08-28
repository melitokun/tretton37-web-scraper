using Tretton37WebScraper.Abstractions;

namespace Tretton37WebScraper;

public static class FileHelper
{
    public static string GetFilePath(HtmlLink htmlLink)
    {
        return htmlLink.IsFile ? htmlLink.Path : $"{htmlLink.Path}/index.html";
    }

    public static string GetFolderPath(HtmlLink htmlLink)
    {
        if (!htmlLink.IsFile)
        {
            return htmlLink.Path;
        }

        var pathArr = htmlLink.Path.Split("/");
        pathArr[^1] = string.Empty;
        return string.Join("/", pathArr);
    }

    public static async Task WriteToFile(Stream stream, HtmlLink htmlLink, string rootFolder)
    {
        var filePath = $"{rootFolder}{GetFilePath(htmlLink)}";
        var folderPath = $"{rootFolder}{GetFolderPath(htmlLink)}";
        
        Directory.CreateDirectory(folderPath);
        
        await using var fileStream = File.Create(filePath);
        stream.Seek(0, SeekOrigin.Begin);
        await stream.CopyToAsync(fileStream);
        fileStream.Close();
    }

    public static async Task WriteToFile(string content, HtmlLink htmlLink, string rootFolder)
    {
        var filePath = $"{rootFolder}{GetFilePath(htmlLink)}";
        var folderPath = $"{rootFolder}{GetFolderPath(htmlLink)}";
        
        Directory.CreateDirectory(folderPath);
        await File.WriteAllTextAsync(filePath, content);
    }
}
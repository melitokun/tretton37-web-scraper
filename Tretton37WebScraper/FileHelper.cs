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
}
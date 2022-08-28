using System.Collections.Concurrent;
using Tretton37WebScraper.Abstractions;

namespace Tretton37WebScraper;

public class WebScraper
{
    private readonly ConcurrentDictionary<string, HtmlLink> _concurrentLinks = new();

    private readonly IHtmlLinkFinder _htmlLinkFinder;
    private readonly IHtmlContentClient _htmlContentClient;

    public WebScraper(IHtmlLinkFinder htmlLinkFinder, IHtmlContentClient htmlContentClient)
    {
        _htmlLinkFinder = htmlLinkFinder;
        _htmlContentClient = htmlContentClient;
    }

    public async Task TraverseAndDownloadAsync(HtmlLink htmlLink, string downloadPath)
    {
        if (_concurrentLinks.ContainsKey(htmlLink.Path))
        {
            await Task.CompletedTask;
        }

        if (_concurrentLinks.TryAdd(htmlLink.Path, htmlLink))
        {
            var filePath = GetFilePath(htmlLink, downloadPath);
            var folderPath = GetFolderPath(htmlLink, downloadPath);

            var content = await _htmlContentClient.GetContentAsString(htmlLink.RawLink);
            var htmlLinks = _htmlLinkFinder.FindAll(content, htmlLink.RawLink).Where(l => !_concurrentLinks.Keys.Contains(l.Path));

            Directory.CreateDirectory(folderPath);
            await File.WriteAllTextAsync(filePath, content);

            var tasks = htmlLinks.Select(async link => await TraverseAndDownloadAsync(link, downloadPath));
            await Task.WhenAll(tasks);
        }
    }

    private string GetFilePath(HtmlLink htmlLink, string downloadPath)
    {
        return htmlLink.IsFile ? $"{downloadPath}{htmlLink.Path}" : $"{downloadPath}{htmlLink.Path}/index.html";
    }

    private string GetFolderPath(HtmlLink htmlLink, string downloadPath)
    {
        if (!htmlLink.IsFile)
        {
            return $"{downloadPath}{htmlLink.Path}";
        }

        var pathArr = htmlLink.Path.Split("/");
        pathArr[^1] = string.Empty;
        return $"{downloadPath}{string.Join("/", pathArr)}";
    }
}
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
            var filePath = $"{downloadPath}{FileHelper.GetFilePath(htmlLink)}";
            var folderPath = $"{downloadPath}{FileHelper.GetFolderPath(htmlLink)}";

            if (htmlLink.IsFile)
            {
                var streamContent = await _htmlContentClient.GetContentAsStream(htmlLink.RawLink);
                await FileHelper.WriteToFile(streamContent, folderPath, filePath);
                await Task.CompletedTask;
            }
            
            var stringContent = await _htmlContentClient.GetContentAsString(htmlLink.RawLink);
            await FileHelper.WriteToFile(stringContent, folderPath, filePath);

            var htmlLinks = _htmlLinkFinder.FindAll(stringContent, htmlLink.RawLink).Where(l => !_concurrentLinks.Keys.Contains(l.Path));
            
            var recursiveTasks = htmlLinks.Select(async link => await TraverseAndDownloadAsync(link, downloadPath));
            await Task.WhenAll(recursiveTasks);
        }
    }
}
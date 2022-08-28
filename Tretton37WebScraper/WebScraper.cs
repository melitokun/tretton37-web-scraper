using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Tretton37WebScraper.Abstractions;

namespace Tretton37WebScraper;

public class WebScraper
{
    private readonly ConcurrentDictionary<string, HtmlLink> _concurrentLinks = new();

    private readonly IHtmlLinkFinder _htmlLinkFinder;
    private readonly IHtmlContentClient _htmlContentClient;
    private readonly ILogger<WebScraper> _logger;

    public WebScraper(IHtmlLinkFinder htmlLinkFinder, IHtmlContentClient htmlContentClient, ILogger<WebScraper> logger)
    {
        _htmlLinkFinder = htmlLinkFinder ?? throw new ArgumentNullException(nameof(htmlLinkFinder));
        _htmlContentClient = htmlContentClient ?? throw new ArgumentNullException(nameof(htmlContentClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task TraverseAndDownloadAsync(HtmlLink htmlLink, string rootFolder)
    {
        if (_concurrentLinks.ContainsKey(htmlLink.Path))
        {
            _logger.LogInformation($"Path '{htmlLink.Path}' is already exists...");
            await Task.CompletedTask;
        }

        if (_concurrentLinks.TryAdd(htmlLink.Path, htmlLink))
        {
            if (htmlLink.IsFile)
            {
                var streamContent = await _htmlContentClient.GetContentAsStream(htmlLink.RawLink);
                _logger.LogInformation($"File '{htmlLink.RawLink}' is downloaded and being saved...");
                await FileHelper.WriteToFile(streamContent, htmlLink, rootFolder);
                await Task.CompletedTask;
            }
            else
            {
                var stringContent = await _htmlContentClient.GetContentAsString(htmlLink.RawLink);
                _logger.LogInformation($"Html content for '{htmlLink.RawLink}' is downloaded and being saved...");
                await FileHelper.WriteToFile(stringContent, htmlLink, rootFolder);

                var htmlLinks = _htmlLinkFinder.FindAll(stringContent, htmlLink.RawLink).Where(l => !_concurrentLinks.Keys.Contains(l.Path));
                _logger.LogInformation($"{htmlLinks.Count()} new html link(s) found...");

                var recursiveTasks = htmlLinks.Select(async link => await TraverseAndDownloadAsync(link, rootFolder));
                await Task.WhenAll(recursiveTasks);
            }
        }
    }
}
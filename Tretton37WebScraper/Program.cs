using Tretton37WebScraper;
using Tretton37WebScraper.Abstractions;

var webScraper =  new WebScraper(new HtmlLinkFinder(), new HtmlContentClient());
await webScraper.TraverseAndDownloadAsync(new HtmlLink
{
    RawLink = "https://tretton37.com",
    Path = "/",
    IsFile = false
}, "./tretton37");
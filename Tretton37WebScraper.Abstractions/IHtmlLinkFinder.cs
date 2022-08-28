namespace Tretton37WebScraper.Abstractions;

public interface IHtmlLinkFinder
{
    IEnumerable<HtmlLink> FindAll(string htmlContent, string baseUrl);
}
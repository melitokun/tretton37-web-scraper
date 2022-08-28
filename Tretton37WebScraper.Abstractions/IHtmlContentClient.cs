namespace Tretton37WebScraper.Abstractions;

public interface IHtmlContentClient
{
    Task<string> GetContentAsString(string url);
}
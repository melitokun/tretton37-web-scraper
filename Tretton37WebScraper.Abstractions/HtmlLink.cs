namespace Tretton37WebScraper.Abstractions;

public class HtmlLink
{
    public string RawLink { get; set; }
    public string Path { get; set; }
    public bool IsFile { get; set; }
}
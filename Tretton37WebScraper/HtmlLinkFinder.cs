using System.Text.RegularExpressions;
using Tretton37WebScraper.Abstractions;

namespace Tretton37WebScraper;

public class HtmlLinkFinder : IHtmlLinkFinder
{
    private readonly Regex _regex = new Regex(@"href=""(/[a-zA-Z\S\.]+)""");
    
    public IEnumerable<HtmlLink> FindAll(string htmlContent, string baseUrl)
    {
        var htmlLinks = new List<HtmlLink>();
        
        var matches = _regex.Matches(htmlContent);
        foreach (Match match in matches)
        {
            var link = match.Groups[1].Value;
            var path = RemoveHashAndQueryStrings(link);
            htmlLinks.Add(new HtmlLink
            {
                RawLink = $"{baseUrl}{link}",
                Path = path,
                IsFile = IsFile(path)
            });
        }

        return htmlLinks;
    }

    public string RemoveHashAndQueryStrings(string link)
    {
        return link.Split("?")[0] .Split("#")[0];
    }

    public bool IsFile(string link)
    {
        return !string.IsNullOrEmpty(Path.GetExtension(link));
    }
}
using Tretton37WebScraper.Abstractions;

namespace Tretton37WebScraper;

public class HtmlContentClient : IHtmlContentClient
{
    private readonly HttpClient _httpClient;
    
    public HtmlContentClient()
    {
        _httpClient = new HttpClient();
    }
    
    public async Task<string> GetContentAsString(string url)
    {
        var responseMessage = await _httpClient.GetAsync(url);
        return await responseMessage.Content.ReadAsStringAsync();
    }

    public async Task<Stream> GetContentAsStream(string url)
    {
        var responseMessage = await _httpClient.GetAsync(url);
        return await responseMessage.Content.ReadAsStreamAsync();
    }
}
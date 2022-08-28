using System.Text.RegularExpressions;

var httpClient = new HttpClient();
var httpResponseMessage = await httpClient.GetAsync("https://tretton37.com");
var content = await httpResponseMessage.Content.ReadAsStringAsync();
// var regex = new Regex(@"href=""(/.[^>]+)""\b");
var regex = new Regex(@"href=""(/[a-zA-Z\S\.]+)""");

var matches = regex.Matches(content);

foreach (Match match in matches)
{
    var link = match.Groups[1];
    var path = link.Value;

    var fileName = Path.GetFileName(link.Value);
    if (!string.IsNullOrEmpty(fileName))
    {
        path = path.Replace($"{fileName}", string.Empty);
    }
    
    Console.WriteLine($"Creating folder: ./tretton37{path}");
    Directory.CreateDirectory($"./tretton37{path}");
}
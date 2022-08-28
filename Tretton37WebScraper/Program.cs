using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Tretton37WebScraper;
using Tretton37WebScraper.Abstractions;

var serviceCollection = new ServiceCollection()
    .AddTransient<WebScraper>()
    .AddTransient<IHtmlLinkFinder, HtmlLinkFinder>()
    .AddTransient<IHtmlContentClient, HtmlContentClient>()
    .AddLogging();

serviceCollection.AddOptions<ConsoleLoggerOptions>().Configure(_ => { });

var serviceProvider = serviceCollection.BuildServiceProvider();
serviceProvider.GetService<ILoggerFactory>()!.AddProvider(new ConsoleLoggerProvider(serviceProvider.GetRequiredService<IOptionsMonitor<ConsoleLoggerOptions>>()));

var webScraper = serviceProvider.GetRequiredService<WebScraper>();
await webScraper.TraverseAndDownloadAsync(new HtmlLink
{
    RawLink = "https://tretton37.com",
    Path = "/",
    IsFile = false
}, "./tretton37");
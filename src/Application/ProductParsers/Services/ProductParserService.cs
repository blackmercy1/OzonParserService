using ErrorOr;
using PuppeteerSharp;

using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Application.ProductParsers.Services;

public class ProductParserService : IProductParserService
{
    public async Task<ErrorOr<ProductData>> ParserAsync(string url)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions {Headless = true});
        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        
        var content = await page.GetContentAsync();
        Console.WriteLine(content);

        var title = await page.EvaluateExpressionAsync<string>("document.title");
        Console.WriteLine($"Заголовок: {title}");
        
        await browser.CloseAsync();
        return new();
    }
}

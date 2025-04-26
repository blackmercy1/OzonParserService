using OzonParserService.Application.ProductScraper;

namespace OzonParserService.Infrastructure.ProductScraper;

public class ProductScraper : IProductScraper
{
    public async Task<string> ScrapeProductDataAsync(string url)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
            
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
            
        await page.GoToAsync(url);
        var content = await page.GetContentAsync();
        
        return content;
    }
}

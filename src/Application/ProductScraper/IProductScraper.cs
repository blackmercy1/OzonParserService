namespace OzonParserService.Application.ProductScraper;

public interface IProductScraper
{
    Task<string> ScrapeProductDataAsync(string url);
}
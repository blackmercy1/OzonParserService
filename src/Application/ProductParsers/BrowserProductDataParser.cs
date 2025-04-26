using OzonParserService.Application.ProductScraper;
using OzonParserService.Domain.ProductDataAggregate;
using OzonParserService.Domain.ProductDataAggregate.ValueObject;

namespace OzonParserService.Application.ProductParsers;

public class BrowserProductDataParser(IProductScraper productScraper, ILogger<BrowserProductDataParser> logger) : IBrowserProductDataParser
{
    public async Task<ErrorOr<ProductData>> ParseAsync(string url)
    {
        var result = await productScraper.ScrapeProductDataAsync(url);

        if (result == string.Empty)
        {
            logger.LogError("Could not scrape product data");
            return Error.Failure("Could not scrape product data");
        }

        return ProductData.Create(
            ProductDataId.CreateUnique(),
            url,
            result,
            100, url);
    }
}

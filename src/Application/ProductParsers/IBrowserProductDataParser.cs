using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Application.ProductParsers;

public interface IBrowserProductDataParser
{
    Task<ErrorOr<ProductData>> ParseAsync(string url);
}

using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Application.ProductParsers.Services;

public interface IProductParserService
{
    Task<ErrorOr<ProductData>> ParserAsync(string url);
}

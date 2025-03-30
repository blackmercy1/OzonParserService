using MassTransit;
using ProductDataContract;

using OzonParserService.Application.Publish;
using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Infrastructure.ProductDataPersistance.Publisher;

public class ProductDataPublisher : IProductDataPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductDataPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public Task PublishProductDataAsync(ProductData productData)
    {
        var productDataMessage = new ProductDataMessage(
            Id: productData.Id.Value,
            ExternalId: productData.ExternalId,
            Name: productData.Name,
            CurrentPrice: productData.CurrentPrice,
            Url: productData.Url);
        
        return _publishEndpoint.Publish(productDataMessage); // todo: сделать saga
    }
}

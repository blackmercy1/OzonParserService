using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Infrastructure.ProductDataPersistence.Publisher;

public class ProductDataPublisher(
    IPublishEndpoint publishEndpoint,
    ITokenGenerator tokenGenerator) : IProductDataPublisher
{
    public async Task PublishProductDataAsync(ProductData productData)
    {
        var token = tokenGenerator.GenerateToken();
        
        var productDataMessage = new ProductDataMessage(
            Id: productData.Id.Value,
            ExternalId: productData.ExternalId,
            Name: productData.Name,
            CurrentPrice: productData.CurrentPrice,
            Url: productData.Url);
        
        await publishEndpoint.Publish(productDataMessage, context =>
        {
            context.Headers.Set("Authorization", $"Bearer {token}");
        });
    }
}

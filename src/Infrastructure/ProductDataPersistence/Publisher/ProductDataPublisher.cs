using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Infrastructure.ProductDataPersistence.Publisher;

public class ProductDataPublisher : IProductDataPublisher
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductDataPublisher(
        IPublishEndpoint publishEndpoint,
        ITokenGenerator tokenGenerator)
    {
        _publishEndpoint = publishEndpoint;
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task PublishProductDataAsync(ProductData productData)
    {
        var token = _tokenGenerator.GenerateToken();
        
        var productDataMessage = new ProductDataMessage(
            Id: productData.Id.Value,
            ExternalId: productData.ExternalId,
            Name: productData.Name,
            CurrentPrice: productData.CurrentPrice,
            Url: productData.Url);
        
        await _publishEndpoint.Publish(productDataMessage, context =>
        {
            context.Headers.Set("Authorization", $"Bearer {token}");
        });
    }
}

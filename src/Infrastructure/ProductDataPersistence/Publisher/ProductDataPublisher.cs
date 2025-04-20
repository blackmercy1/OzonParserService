using MassTransit;
using OzonParserService.Application.Common.Authentication;
using OzonParserService.Application.Publish;
using OzonParserService.Domain.ProductDataAggregate;
using ProductDataContract;

namespace OzonParserService.Infrastructure.ProductDataPersistence.Publisher;

public class ProductDataPublisher : IProductDataPublisher
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductDataPublisher(
        IPublishEndpoint publishEndpoint,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _publishEndpoint = publishEndpoint;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task PublishProductDataAsync(ProductData productData)
    {
        var token = _jwtTokenGenerator.GenerateToken();
        
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

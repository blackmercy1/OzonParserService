using OzonParserService.Domain.ProductDataAggregate.ValueObject;

namespace OzonParserService.Domain.ProductDataAggregate;

public class ProductData : AggregateRoot<ProductDataId>
{
    public string ExternalId { get; private set; }
    public string Name { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public string Url { get; private set; }

    public ProductData(string externalId, string name, decimal currentPrice, string url)
    {
        ExternalId = externalId;
        Name = name;
        CurrentPrice = currentPrice;
        Url = url;
    }
    
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ExternalId)
               && CurrentPrice > 0
               && !string.IsNullOrWhiteSpace(Name)
               && Uri.TryCreate(Url, UriKind.Absolute, out _);
    }
}

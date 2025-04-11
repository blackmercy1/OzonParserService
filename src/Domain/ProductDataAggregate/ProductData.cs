using OzonParserService.Domain.ProductDataAggregate.ValueObject;

namespace OzonParserService.Domain.ProductDataAggregate;

public class ProductData : AggregateRoot<ProductDataId>
{
    public string ExternalId { get; private set; }
    public string Name { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public string Url { get; private set; }

    private ProductData(
        ProductDataId id,
        string externalId,
        string name,
        decimal currentPrice,
        string url)
    {
        Id = id;
        ExternalId = externalId;
        Name = name;
        CurrentPrice = currentPrice;
        Url = url;
    }

    public static ProductData Create(
        ProductDataId id,
        string externalId,
        string name,
        decimal currentPrice,
        string url)
    {
        return new ProductData(
            id: id,
            externalId: externalId,
            name: name,
            currentPrice: currentPrice,
            url: url);
    }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ExternalId)
               && CurrentPrice > 0
               && !string.IsNullOrWhiteSpace(Name)
               && Uri.TryCreate(Url, UriKind.Absolute, out _);
    }
}

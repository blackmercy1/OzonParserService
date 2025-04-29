using Newtonsoft.Json;

namespace OzonParserService.Domain.ProductDataAggregate.ValueObject;

public class ProductDataId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    [JsonConstructor]
    private ProductDataId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static ProductDataId Create(Guid value) => new(value);

    public static ProductDataId CreateUnique() => new(Guid.NewGuid());
}

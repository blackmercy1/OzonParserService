namespace OzonParserService.Domain.ParserTaskAggregate.ValueObject;

public class ParsingTaskId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    private ParsingTaskId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static ParsingTaskId Create(Guid value) => new(value);

    public static ParsingTaskId CreateUnique() => new(Guid.NewGuid());
}


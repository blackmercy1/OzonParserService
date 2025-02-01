using OzonParserService.Domain.Common;

namespace OzonParserService.Domain.ParserTaskAggregate;

public class ParserTaskId : AggregateRootId<Guid>
{
    public sealed override Guid Value { get; protected set; }

    private ParserTaskId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public static ParserTaskId Create(Guid value) => new(value);

    public static ParserTaskId CreateUnique() => new(Guid.NewGuid());
}


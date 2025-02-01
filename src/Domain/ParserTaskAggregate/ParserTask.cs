using OzonParserService.Domain.Common;

namespace OzonParserService.Domain.ParserTaskAggregate;

public class ParserTask : AggregateRoot<ParserTaskId>
{
    public string ProductUrl { get; private set; }
    public string ExternalProductId { get; private set; }
    public TimeSpan CheckInterval { get; private set; }
    public decimal Price { get; private set; }
    
    public ParserTaskStatus Status { get; private set; }
    public DateTime LastRun { get; private set; }
    public DateTime NextRun { get; private set; }
    
    private ParserTask(
        ParserTaskId id,
        string productUrl,
        string externalProductId,
        TimeSpan checkInterval,
        ParserTaskStatus status,
        DateTime lastRun,
        DateTime nextRun,
        decimal price) : base(id)
    {
        ProductUrl = productUrl;
        ExternalProductId = externalProductId;
        CheckInterval = checkInterval;
        Status = status;
        LastRun = lastRun;
        NextRun = nextRun;
        Price = price;
    }

    public static ParserTask Create(
        string productUrl,
        decimal price,
        TimeSpan checkInterval) => new(
        id: ParserTaskId.CreateUnique(), 
        productUrl: productUrl,
        price: price,
        externalProductId: ExtractExternalId(productUrl),
        checkInterval: checkInterval,
        status: ParserTaskStatus.Scheduled,
        lastRun: DateTime.Now,
        nextRun: DateTime.Now.Add(checkInterval)
    );
    
    public void Start()
    {
        Status = ParserTaskStatus.Running;
        LastRun = DateTime.UtcNow;
        // AddDomainEvent(new ParserTaskStartedEvent(Id));
    }

    public void Complete()
    {
        Status = ParserTaskStatus.Completed;
        NextRun = DateTime.UtcNow.Add(CheckInterval);
        // AddDomainEvent(new ParserTaskCompletedEvent(Id));
    }

    public void Fail(string error)
    {
        Status = ParserTaskStatus.Failed;
        // AddDomainEvent(new ParserTaskFailedEvent(Id, error));
    } 
    
    private static string ExtractExternalId(string url) => url.Split('/').Last();

#pragma warning disable CS8618
    private ParserTask(
        )
    {
    }
#pragma warning restore CS8618
}

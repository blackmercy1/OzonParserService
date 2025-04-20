using OzonParserService.Domain.ParserTaskAggregate.DomainEvents;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Domain.ParserTaskAggregate;

public class ParsingTask : AggregateRoot<ParsingTaskId>
{
    public string ProductUrl { get; private set; }
    public string ExternalProductId { get; private set; }
    public TimeSpan CheckInterval { get; private set; }
    public ParserTaskStatus Status { get; private set; }

    public DateTime LastRun { get; private set; }
    public DateTime NextRun { get; private set; }

    private ParsingTask(
        ParsingTaskId id,
        string productUrl,
        string externalProductId,
        TimeSpan checkInterval)
        : base(id)
    {
        ProductUrl = productUrl;
        CheckInterval = checkInterval;
        ExternalProductId = externalProductId;
        Status = ParserTaskStatus.Scheduled;
        LastRun = DateTime.UtcNow;
        NextRun = CalculateNextRun();

        AddDomainEvent(new ParserTaskCreatedEvent(Id));
    }

    public static ParsingTask Create(
        string productUrl,
        TimeSpan checkInterval)
    {
        if (!IsValidOzonUrl(productUrl))
            throw new ArgumentException(
                "Invalid Ozon URL",
                nameof(productUrl));

        var externalId = ExtractExternalId(productUrl);

        return new ParsingTask(
            id: ParsingTaskId.CreateUnique(),
            productUrl: productUrl,
            externalProductId: externalId,
            checkInterval: checkInterval
        );
    }

    public void Start()
    {
        if (Status != ParserTaskStatus.Scheduled)
            throw new InvalidOperationException("Task can only be started from Scheduled state");

        Status = ParserTaskStatus.Running;
        LastRun = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = ParserTaskStatus.Completed;
        NextRun = CalculateNextRun();
    }

    public void UpdateLastRunTime(DateTime time) => LastRun = time;

    public void Fail(string error)
    {
        Status = ParserTaskStatus.Failed;
    }

    private DateTime CalculateNextRun() => DateTime.UtcNow.Add(CheckInterval);

    private static string ExtractExternalId(string url)
    {
        var parts = url.Split('/');
        if (parts.Length == 0)
            throw new ArgumentException("URL does not contain product ID");

        return parts.Last();
    }

    private static bool IsValidOzonUrl(string url) =>
        Uri.TryCreate(
            url,
            UriKind.Absolute,
            out var uri) && uri.Host.Contains("ozon.ru");

#pragma warning disable CS8618
    private ParsingTask()
    { }
#pragma warning restore CS8618
}

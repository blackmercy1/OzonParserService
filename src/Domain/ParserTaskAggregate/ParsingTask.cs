using System.Text.RegularExpressions;
using OzonParserService.Domain.ParserTaskAggregate.DomainEvents;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;
using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Domain.ParserTaskAggregate;

public partial class ParsingTask : AggregateRoot<ParsingTaskId>
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
        TimeSpan checkInterval,
        DateTime utcNow)
        : base(id)
    {
        ProductUrl = productUrl;
        CheckInterval = checkInterval;
        ExternalProductId = externalProductId;
        Status = ParserTaskStatus.Scheduled;
        LastRun = utcNow;
        NextRun = CalculateNextRun(utcNow);

        AddDomainEvent(new ParserTaskCreatedEvent(Id));
    }

    public static ParsingTask Create(
        string productUrl,
        TimeSpan checkInterval,
        DateTime utcNow)
    {
        if (checkInterval <= TimeSpan.Zero)
            throw new ArgumentException("Check interval must be positive.", nameof(checkInterval));
        
        if (!IsValidOzonUrl(productUrl))
            throw new ArgumentException("Invalid Ozon URL", nameof(productUrl));

        var externalIdResult = ExtractExternalId(productUrl);
        
        if (externalIdResult.IsError)
            throw new Exception(externalIdResult.Errors.ToString());
        
        return new ParsingTask(
            id: ParsingTaskId.CreateUnique(),
            productUrl: productUrl,
            externalProductId: externalIdResult.Value,
            checkInterval: checkInterval,
            utcNow: utcNow
        );
    }

    public ErrorOr<Success> Start()
    {
        Status = ParserTaskStatus.Running;
        LastRun = DateTime.UtcNow;

        AddDomainEvent(new ParserTaskStartedEvent(Id));
        return Result.Success;
    }

    public ErrorOr<Success> Complete(
        ProductData productData,
        DateTime utcNow)
    {
        Status = ParserTaskStatus.Completed;
        NextRun = CalculateNextRun(utcNow);

        AddDomainEvent(new ParserTaskCompletedEvent(Id, productData));
        
        return Result.Success;
    }

    public ErrorOr<Success> Fail(string error)
    {
        Status = ParserTaskStatus.Failed;
        AddDomainEvent(new ParserTaskFailedEvent(Id, error));
        return Error.Failure(description: error);
    }

    private DateTime CalculateNextRun(DateTime currentTime) => currentTime.Add(CheckInterval);

    private static ErrorOr<string> ExtractExternalId(string url)
    {
        var regex = OzonRegex();
        var match = regex.Match(url);
        if (!match.Success)
            return Error.Failure("URL does not contain product ID");
        return match.Groups[1].Value;
    }

    private static bool IsValidOzonUrl(string url) =>
        Uri.TryCreate(
            url,
            UriKind.Absolute,
            out var uri)
        && uri.Host.Contains("ozon.ru");

#pragma warning disable CS8618
    private ParsingTask()
    { }
#pragma warning restore CS8618
    [GeneratedRegex(@"ozon\.ru/.*?/(\d+)/")]
    private static partial Regex OzonRegex();
}

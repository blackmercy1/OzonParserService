namespace OzonParserService.Contracts.ParsingTask;

public record ParsingTaskResponse(
    string ProductUrl,
    string ExternalProductId,
    TimeSpan CheckInterval,
    string Status,
    DateTime LastRun,
    DateTime NextRun);

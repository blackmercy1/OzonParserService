namespace OzonParserService.Contracts.ParsingTask;

public record CreateParsingTaskRequest(
    string ProductUrl,
    TimeSpan IntervalHours);

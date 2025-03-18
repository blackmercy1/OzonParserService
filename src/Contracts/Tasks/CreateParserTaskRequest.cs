namespace OzonParserService.Contracts.Tasks;

public record CreateParserTaskRequest(
    string ProductUrl,
    TimeSpan IntervalHours);

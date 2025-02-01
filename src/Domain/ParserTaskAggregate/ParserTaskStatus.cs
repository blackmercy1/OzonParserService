namespace OzonParserService.Domain.ParserTaskAggregate;

public enum ParserTaskStatus
{
    Scheduled,
    Running,
    Completed,
    Failed,
    Disabled
}
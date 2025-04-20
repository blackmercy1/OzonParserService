using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Commands;

public record CreateParsingTaskCommand(
    string ProductUrl,
    TimeSpan IntervalHours) : IRequest<ErrorOr<ParsingTask>>;

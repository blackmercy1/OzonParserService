using ErrorOr;
using MediatR;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Commands;

public record CreateParserTaskCommand(
    string ProductUrl,
    TimeSpan IntervalHours) : IRequest<ErrorOr<ParsingTask>>;

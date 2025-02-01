using MediatR;
using OzonParserService.Domain.ParserTaskAggregate;
using ErrorOr;

namespace OzonParserService.Application.Tasks.Commands;

public record CreateParserTaskCommand(
    string ProductUrl,
    int IntervalHours) : IRequest<ErrorOr<ParserTask>>;

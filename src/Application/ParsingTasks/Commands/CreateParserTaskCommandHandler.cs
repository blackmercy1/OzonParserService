using ErrorOr;
using MediatR;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Commands;

public class CreateParserTaskCommandHandler
    : IRequestHandler<CreateParserTaskCommand, ErrorOr<ParsingTask>>
{
    private readonly IParsingTaskService _parsingTaskService;

    public CreateParserTaskCommandHandler(IParsingTaskService parsingTaskService)
    {
        _parsingTaskService = parsingTaskService;
    }
    
    public async Task<ErrorOr<ParsingTask>> Handle(
        CreateParserTaskCommand request,
        CancellationToken cancellationToken)
    {
        var parsingTask = await _parsingTaskService.ScheduleTaskAsync(
            url: request.ProductUrl,
            interval: request.IntervalHours, 
            cancellationToken: cancellationToken);
        
        return Error.Failure("some", "some");
    }
}

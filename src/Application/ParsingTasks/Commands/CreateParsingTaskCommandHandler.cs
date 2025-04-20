using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Commands;

public class CreateParsingTaskCommandHandler
    : IRequestHandler<CreateParsingTaskCommand, ErrorOr<ParsingTask>>
{
    private readonly IParsingTaskService _parsingTaskService;

    public CreateParsingTaskCommandHandler(IParsingTaskService parsingTaskService)
    {
        _parsingTaskService = parsingTaskService;
    }
    
    public async Task<ErrorOr<ParsingTask>> Handle(
        CreateParsingTaskCommand request,
        CancellationToken cancellationToken)
    {
        var parsingTask = await _parsingTaskService.ScheduleTaskAsync(
            url: request.ProductUrl,
            interval: request.IntervalHours, 
            cancellationToken: cancellationToken);
        
        return parsingTask;
    }
}

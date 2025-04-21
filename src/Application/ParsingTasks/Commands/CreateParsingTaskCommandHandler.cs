using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Commands;

public class CreateParsingTaskCommandHandler(IParsingTaskService parsingTaskService) : IRequestHandler<CreateParsingTaskCommand, ErrorOr<ParsingTask>>
{
    public async Task<ErrorOr<ParsingTask>> Handle(
        CreateParsingTaskCommand request,
        CancellationToken cancellationToken)
    {
        var parsingTask = await parsingTaskService.ScheduleTaskAsync(
            url: request.ProductUrl,
            interval: request.IntervalHours, 
            cancellationToken: cancellationToken);
        
        return parsingTask;
    }
}

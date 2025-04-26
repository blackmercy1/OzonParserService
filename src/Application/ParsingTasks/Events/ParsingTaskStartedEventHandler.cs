using OzonParserService.Application.Common.DateTimeProvider;
using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers;
using OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

namespace OzonParserService.Application.ParsingTasks.Events;

[UsedImplicitly]
public class ParsingTaskStartedEventHandler(
    IBrowserProductDataParser browserProductDataParser,
    IParsingTaskRepository parsingTaskRepository,
    IDateTimeProvider dateTimeProvider,
    ILogger<ParsingTaskService> logger) : INotificationHandler<ParserTaskStartedEvent>
{
    public async Task Handle(
        ParserTaskStartedEvent notification,
        CancellationToken cancellationToken)
    {
        var parsingTask = notification.ParsingTask;

        var productDataResult = await browserProductDataParser.ParseAsync(parsingTask.ProductUrl);
        if (productDataResult.IsError)
        {
            var errors = productDataResult.Errors;
            logger.LogError(errors.ToString());
        }
        
        await parsingTaskRepository.UpdateByIdAsync(parsingTask, parsingTask.Id, cancellationToken);
        await parsingTaskRepository.SaveChangesAsync(cancellationToken);

        parsingTask.Complete(productDataResult.Value, dateTimeProvider.UtcNow);
    }
}

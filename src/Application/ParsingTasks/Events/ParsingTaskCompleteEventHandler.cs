using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Application.Publish;
using OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

namespace OzonParserService.Application.ParsingTasks.Events;

[UsedImplicitly]
public class ParsingTaskCompleteEventHandler(
    IParsingTaskRepository parsingTaskRepository,
    IProductDataPublisher productDataPublisher) : INotificationHandler<ParserTaskCompletedEvent>
{
    public async Task Handle(
        ParserTaskCompletedEvent notification,
        CancellationToken cancellationToken)
    {
        var parsingTask = notification.ParsingTask;
        var productData = notification.ProductData;
        
        await parsingTaskRepository.UpdateByIdAsync(parsingTask, parsingTask.Id, cancellationToken);
        await parsingTaskRepository.SaveChangesAsync(cancellationToken);
        
        await productDataPublisher.PublishProductDataAsync(productData);
    }
}
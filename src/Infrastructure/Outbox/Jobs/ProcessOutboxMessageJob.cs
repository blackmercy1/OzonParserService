using Newtonsoft.Json;
using OzonParserService.Application.Outbox.Jobs;
using OzonParserService.Application.Outbox.Repository;

namespace OzonParserService.Infrastructure.Outbox.Jobs;

public class ProcessOutboxMessageJob(
    IOutboxRepository<OutboxMessage> outboxRepository,
    IPublisher publisher,
    ILogger<ProcessOutboxMessageJob> logger) : IProcessOutboxJobMessagesJob
{
    public async Task ProcessOutboxMessages(CancellationToken cancellationToken)
    {
        var messages = await outboxRepository.GetUnprocessedAsync(
            100,
            cancellationToken);

        var serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject(
                    message.Content, 
                    typeof(IDomainEvent), 
                    serializerSettings) as IDomainEvent;

                if (domainEvent is null)
                    throw new JsonException($"Failed to deserialize message {message.Id}");

                await publisher.Publish(domainEvent, cancellationToken);
                await outboxRepository.MarkAsProcessedAsync(message.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                await outboxRepository.MarkAsFailedAsync(
                    message.Id,
                    ex.ToString(),
                    cancellationToken);
            
                logger.LogError(
                    ex,
                    "Error processing outbox message with id {MessageId}",
                    message.Id);
            }
        }
    }
}

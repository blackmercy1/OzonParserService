namespace OzonParserService.Application.Outbox.Jobs;

public interface IProcessOutboxJobMessagesJob
{
    Task ProcessOutboxMessages(CancellationToken cancellationToken);
}

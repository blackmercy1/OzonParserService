namespace OzonParserService.Application.Outbox.Repository;

public interface IOutboxRepository<T>
    where T : class
{
    Task AddAsync(
        T message, 
        CancellationToken cancellationToken = default);

    Task<List<T>> GetUnprocessedAsync(
        int batchSize = 100,
        CancellationToken cancellationToken = default);
    
    Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken cancellationToken = default);

    Task CleanProcessedAsync(
        DateTime olderThanUtc,
        CancellationToken cancellationToken = default);
}

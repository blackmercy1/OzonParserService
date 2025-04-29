using OzonParserService.Application.Outbox.Repository;

namespace OzonParserService.Infrastructure.Outbox.Persistence
{
    public sealed class OutboxRepository(
        OzonDbContext dbContext,
        ILogger<OutboxRepository> logger) : IOutboxRepository<OutboxMessage>
    {
        public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
        {
            await dbContext
            .Set<OutboxMessage>()
            .AddAsync(message, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogDebug(
                "Added outbox message {MessageId}",
                message.Id);
        }

        public async Task<List<OutboxMessage>> GetUnprocessedAsync(
            int batchSize = 100,
            CancellationToken cancellationToken = default) =>
            await dbContext
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccuredOnUtc)
                .Take(batchSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task MarkAsProcessedAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var message = await dbContext
                .Set<OutboxMessage>()
                .FirstOrDefaultAsync(
                    m => m.Id == id,
                    cancellationToken);

            if (message != null)
            {
                message.ProcessedOnUtc = DateTime.UtcNow;
                message.Error = null;
                await dbContext.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Marked message {MessageId} as processed", id);
            }
        }

        public async Task MarkAsFailedAsync(
            Guid id,
            string error,
            CancellationToken cancellationToken = default)
        {
            var message = await dbContext
                .Set<OutboxMessage>()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (message != null)
            {
                message.Error = error;
                await dbContext.SaveChangesAsync(cancellationToken);

                logger.LogError("Marked message {MessageId} as failed. Error: {Error}",
                    id,
                    error);
            }
        }

        public async Task CleanProcessedAsync(
            DateTime olderThanUtc,
            CancellationToken cancellationToken = default)
        {
            var messagesToDelete = await dbContext
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc != null && m.ProcessedOnUtc < olderThanUtc)
                .ToListAsync(cancellationToken);

            if (messagesToDelete.Count > 0)
            {
                dbContext.Set<OutboxMessage>().RemoveRange(messagesToDelete);
                await dbContext.SaveChangesAsync(cancellationToken);

                logger.LogInformation(
                    "Cleaned {Count} processed outbox messages",
                    messagesToDelete.Count);
            }
        }
    }
}

using Newtonsoft.Json;
using OzonParserService.Infrastructure.Outbox;

namespace OzonParserService.Infrastructure.Persistence.Interceptors;

public class PublishOutboxMessagesInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ProcessOutboxMessages(eventData.Context)
            .GetAwaiter()
            .GetResult();

        return base.SavingChanges(
            eventData,
            result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await ProcessOutboxMessages(eventData.Context);

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    private Task ProcessOutboxMessages(DbContext? dbContext)
    {
        if (dbContext is null)
            return Task.CompletedTask;

        var entitiesWithDomainEvents = dbContext
            .ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();
        
        var outboxMessages = entitiesWithDomainEvents
            .SelectMany(domainEvent => domainEvent.DomainEvents)
            .Select(domainEvent => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = dateTimeProvider.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
            })
            .ToList();

        entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return Task.CompletedTask;
    }
}

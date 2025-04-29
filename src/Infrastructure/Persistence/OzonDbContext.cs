using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Infrastructure.Outbox;
using OzonParserService.Infrastructure.Persistence.Interceptors;

namespace OzonParserService.Infrastructure.Persistence;

public class OzonDbContext(
    PublishOutboxMessagesInterceptor publishOutboxMessagesInterceptor,
    DbContextOptions<OzonDbContext> options) : DbContext(options)
{
    public DbSet<ParsingTask> ParsingTasks { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(OzonDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(publishOutboxMessagesInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}

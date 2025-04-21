using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Infrastructure.Persistence;

public class OzonDbContext(DbContextOptions<OzonDbContext> options) : DbContext(options)
{
    public DbSet<ParsingTask> ParsingTasks { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OzonDbContext).Assembly);
    }
}

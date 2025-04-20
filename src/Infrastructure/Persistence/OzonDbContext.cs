using Microsoft.EntityFrameworkCore;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Infrastructure.Persistence;

public class OzonDbContext : DbContext
{
    public DbSet<ParsingTask> ParsingTasks { get; set; }
    
    public OzonDbContext(DbContextOptions<OzonDbContext> options) : base(options)
    { }
    
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OzonDbContext).Assembly);
    }
}

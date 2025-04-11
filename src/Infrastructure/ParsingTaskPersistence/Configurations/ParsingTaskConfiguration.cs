using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class ParsingTaskConfiguration : IEntityTypeConfiguration<ParsingTask>
{
    public void Configure(
        EntityTypeBuilder<ParsingTask> builder)
    {
        ConfigureParsingTaskTable(builder);
    }

    private void ConfigureParsingTaskTable(
        EntityTypeBuilder<ParsingTask> builder)
    {
        builder
            .ToTable("parser_tasks");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ParsingTaskId.Create(value));

        builder
            .Property(x => x.ProductUrl)
            .IsRequired();
        
        builder
            .Property(x => x.CheckInterval)
            .IsRequired();
        
        builder
            .Property(x => x.Status)
            .IsRequired();
        
        builder
            .Property(x => x.LastRun)
            .IsRequired();
        
        builder
            .Property(x => x.NextRun)
            .IsRequired();
    }
}

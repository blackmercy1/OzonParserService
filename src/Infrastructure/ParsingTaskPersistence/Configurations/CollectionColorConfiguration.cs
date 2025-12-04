using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class CollectionColorConfiguration : IEntityTypeConfiguration<CollectionColor>
{
    public void Configure(EntityTypeBuilder<CollectionColor> builder)
    {
        builder.ToTable("collection_color");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("crm_id")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, v => CollectionColorId.Create(v));

        builder.Property(x => x.Name).IsRequired().HasColumnName("name");
        builder.Property(x => x.CodeColor).HasColumnName("code_color");
        builder.Property(x => x.InitialColor).HasColumnName("initialcolor");
        builder.Property(x => x.FinalColor).HasColumnName("finalcolor");
        builder.Property(x => x.Active).HasColumnName("active");
    }
}
using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class SegmentationTypeConfiguration : IEntityTypeConfiguration<SegmentationType>
{
    public void Configure(EntityTypeBuilder<SegmentationType> builder)
    {
        builder.ToTable("segmentation_type");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("crm_id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                v => SegmentationTypeId.Create(v));

        builder.Property(x => x.Name).IsRequired().HasColumnName("name");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Active).HasColumnName("active");
    }
}
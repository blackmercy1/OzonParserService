using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class SegmentationConfiguration : IEntityTypeConfiguration<Segmentation>
{
    public void Configure(EntityTypeBuilder<Segmentation> builder)
    {
        builder.ToTable("segmentations");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, v => SegmentationId.Create(v));

        builder
            .Property(x => x.GuidReq)
            .HasColumnName("guid_req")
            .HasConversion(id => id.Value, v => LoginomReqId.Create(v));

        builder.Property(x => x.Parameter).HasColumnName("parametr");
        builder.Property(x => x.Value).HasColumnName("value");
        builder.Property(x => x.Hash).HasColumnName("hash");
        builder.Property(x => x.Score).HasColumnName("score");
        builder.Property(x => x.Section).HasColumnName("section");

        builder
            .Property(x => x.SegmentationTypeId)
            .HasColumnName("segmentation_type_crm_id")
            .HasConversion(id => id.Value, v => SegmentationTypeId.Create(v));

        builder
            .HasOne(x => x.SegmentationType)
            .WithMany()
            .HasForeignKey(nameof(Segmentation.SegmentationTypeId))
            .HasPrincipalKey(x => x.Id);
    }
}
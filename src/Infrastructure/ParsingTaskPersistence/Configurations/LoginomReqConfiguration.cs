using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class LoginomReqConfiguration : IEntityTypeConfiguration<LoginomReq>
{
    public void Configure(EntityTypeBuilder<LoginomReq> builder)
    {
        builder.ToTable("loginom_req");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("guid_req")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, v => LoginomReqId.Create(v));

        builder.Property(x => x.GuidAccount).HasColumnName("guid_account");
        builder.Property(x => x.SegmentationScore).HasColumnName("segmentation_score");
        builder.Property(x => x.TrafficLightMax).HasColumnName("traffic_light_max");
        builder.Property(x => x.RecalculationDate).HasColumnName("recalculation_date");
        builder.Property(x => x.Hash).HasColumnName("hash");
    }
}
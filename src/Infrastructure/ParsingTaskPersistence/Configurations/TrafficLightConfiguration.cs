using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class TrafficLightConfiguration : IEntityTypeConfiguration<TrafficLight>
{
    public void Configure(EntityTypeBuilder<TrafficLight> builder)
    {
        builder.ToTable("traffic_light");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, v => TrafficLightId.Create(v));

        builder
            .Property(x => x.GuidReq)
            .HasColumnName("guid_req")
            .HasConversion(id => id.Value, v => LoginomReqId.Create(v));

        builder.Property(x => x.Parameter).HasColumnName("parametr");
        builder.Property(x => x.Value).HasColumnName("value");
        builder.Property(x => x.TrafficLightMax).HasColumnName("traffic_light_max");
        builder.Property(x => x.Hash).HasColumnName("hash");
        builder.Property(x => x.TrafficLightScore).HasColumnName("traffic_light_score");
        builder.Property(x => x.Section).HasColumnName("section");

        builder
            .Property(x => x.CollectionColorId)
            .HasColumnName("collection_color_crm_id")
            .HasConversion(
                id => id != null ? id.Value : null,
                v => v != null ? CollectionColorId.Create(v) : null);

        builder
            .Property(x => x.TrafficLightTypeId)
            .HasColumnName("traffic_light_type_crm_id")
            .HasConversion(
                id => id != null ? id.Value : null,
                v => v != null ? TrafficLightTypeId.Create(v) : null);

        builder
            .HasOne(x => x.CollectionColor)
            .WithMany()
            .HasForeignKey(nameof(TrafficLight.CollectionColorId))
            .HasPrincipalKey(x => x.Id);

        builder
            .HasOne(x => x.TrafficLightType)
            .WithMany()
            .HasForeignKey(nameof(TrafficLight.TrafficLightTypeId))
            .HasPrincipalKey(x => x.Id);
    }
}
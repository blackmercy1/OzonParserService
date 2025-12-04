using OzonParserService.Domain.ConveerCollection;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Configurations;

public class TrafficLightTypeConfiguration : IEntityTypeConfiguration<TrafficLightType>
{
    public void Configure(EntityTypeBuilder<TrafficLightType> builder)
    {
        builder.ToTable("traffic_light_type");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("crm_id")
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, v => TrafficLightTypeId.Create(v));

        builder.Property(x => x.Name).IsRequired().HasColumnName("name");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Active).HasColumnName("active");
    }
}
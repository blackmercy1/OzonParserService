namespace OzonParserService.Domain.ConveerCollection;

public partial class TrafficLightType : AggregateRoot<TrafficLightTypeId>
{
    public string Name { get; private set; }

    public int Code { get; private set; }

    public bool Active { get; private set; }

    private TrafficLightType(
        TrafficLightTypeId id,
        string name,
        int code,
        bool active) : base(id)
    {
        Name = name;
        Code = code;
        Active = active;
    }

    public static TrafficLightType Create(
        string crmId,
        string name,
        int code,
        bool active)
        => new TrafficLightType(
            TrafficLightTypeId.Create(crmId),
            name,
            code,
            active);

#pragma warning disable CS8618
    private TrafficLightType() { }
#pragma warning restore CS8618
}
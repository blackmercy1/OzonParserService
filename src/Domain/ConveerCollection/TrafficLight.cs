namespace OzonParserService.Domain.ConveerCollection;

public partial class TrafficLight : AggregateRoot<TrafficLightId>
{
    public LoginomReqId GuidReq { get; private set; }

    public string Parameter { get; private set; }

    public string Value { get; private set; }

    public int? TrafficLightMax { get; private set; }

    public string? Hash { get; private set; }

    public int? TrafficLightScore { get; private set; }

    public string Section { get; private set; }

    public CollectionColorId? CollectionColorId { get; private set; }

    public CollectionColor? CollectionColor { get; private set; }

    public TrafficLightTypeId? TrafficLightTypeId { get; private set; }

    public TrafficLightType? TrafficLightType { get; private set; }

    private TrafficLight(
        TrafficLightId id,
        LoginomReqId guidReq,
        string parameter,
        string value,
        int? trafficLightMax,
        string? hash,
        int? trafficLightScore,
        string section,
        CollectionColorId? collectionColorId,
        TrafficLightTypeId? trafficLightTypeId)
        : base(id)
    {
        GuidReq = guidReq;
        Parameter = parameter;
        Value = value;
        TrafficLightMax = trafficLightMax;
        Hash = hash;
        TrafficLightScore = trafficLightScore;
        Section = section;
        CollectionColorId = collectionColorId;
        TrafficLightTypeId = trafficLightTypeId;
    }

    public static TrafficLight Create(
        string id,
        string guidReq,
        string parameter,
        string value,
        int? trafficLightMax,
        string? hash,
        int? trafficLightScore,
        string section,
        string? collectionColorCrmId,
        string? trafficLightTypeCrmId)
        => new TrafficLight(
            TrafficLightId.Create(id),
            LoginomReqId.Create(guidReq),
            parameter,
            value,
            trafficLightMax,
            hash,
            trafficLightScore,
            section,
            collectionColorCrmId != null ? CollectionColorId.Create(collectionColorCrmId) : null,
            trafficLightTypeCrmId != null ? TrafficLightTypeId.Create(trafficLightTypeCrmId) : null
        );

#pragma warning disable CS8618
    private TrafficLight() { }
#pragma warning restore CS8618
}
namespace OzonParserService.Domain.ConveerCollection;

public partial class LoginomReq : AggregateRoot<LoginomReqId>
{
    public string GuidAccount { get; private set; }

    public int? SegmentationScore { get; private set; }

    public int? TrafficLightMax { get; private set; }

    public DateTime? RecalculationDate { get; private set; }

    public string? Hash { get; private set; }

    // nav
    public ICollection<Segmentation> Segmentations { get; private set; } = new List<Segmentation>();

    public ICollection<TrafficLight> TrafficLights { get; private set; } = new List<TrafficLight>();

    private LoginomReq(
        LoginomReqId id,
        string guidAccount,
        int? segmentationScore,
        int? trafficLightMax,
        DateTime? recalculationDate,
        string? hash)
        : base(id)
    {
        GuidAccount = guidAccount;
        SegmentationScore = segmentationScore;
        TrafficLightMax = trafficLightMax;
        RecalculationDate = recalculationDate;
        Hash = hash;
    }

    public static LoginomReq Create(
        string guidReq,
        string guidAccount,
        int? segmentationScore,
        int? trafficLightMax,
        DateTime? recalculationDate,
        string? hash)
        => new LoginomReq(
            LoginomReqId.Create(guidReq),
            guidAccount,
            segmentationScore,
            trafficLightMax,
            recalculationDate,
            hash);

#pragma warning disable CS8618
    private LoginomReq() { }
#pragma warning restore CS8618
}
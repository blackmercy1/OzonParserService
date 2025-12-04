namespace OzonParserService.Domain.ConveerCollection;

public partial class Segmentation : AggregateRoot<SegmentationId>
{
    public LoginomReqId GuidReq { get; private set; }

    public string Parameter { get; private set; }

    public string Value { get; private set; }

    public string Hash { get; private set; }

    public int? Score { get; private set; }

    public string Section { get; private set; }

    // FK to segmentation_type (crm_id)
    public SegmentationTypeId SegmentationTypeId { get; private set; }

    public SegmentationType? SegmentationType { get; private set; }

    private Segmentation(
        SegmentationId id,
        LoginomReqId guidReq,
        string parameter,
        string value,
        string hash,
        int? score,
        string section,
        SegmentationTypeId segmentationTypeId)
        : base(id)
    {
        GuidReq = guidReq;
        Parameter = parameter;
        Value = value;
        Hash = hash;
        Score = score;
        Section = section;
        SegmentationTypeId = segmentationTypeId;
    }

    public static Segmentation Create(
        string id,
        string guidReq,
        string parameter,
        string value,
        string hash,
        int? score,
        string section,
        string segmentationTypeCrmId)
        => new Segmentation(
            SegmentationId.Create(id),
            LoginomReqId.Create(guidReq),
            parameter,
            value,
            hash,
            score,
            section,
            SegmentationTypeId.Create(segmentationTypeCrmId));

#pragma warning disable CS8618
    private Segmentation() { }
#pragma warning restore CS8618
}
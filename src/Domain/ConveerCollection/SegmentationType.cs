namespace OzonParserService.Domain.ConveerCollection;

public partial class SegmentationType : AggregateRoot<SegmentationTypeId>
{
    public string Name { get; private set; }

    public int Code { get; private set; }

    public bool Active { get; private set; }

    private SegmentationType(
        SegmentationTypeId id,
        string name,
        int code,
        bool active)
        : base(id)
    {
        Name = name;
        Code = code;
        Active = active;
    }

    public static SegmentationType Create(
        string crmId,
        string name,
        int code,
        bool active)
        => new SegmentationType(
            SegmentationTypeId.Create(crmId),
            name,
            code,
            active);

#pragma warning disable CS8618
    private SegmentationType() { }
#pragma warning restore CS8618
}
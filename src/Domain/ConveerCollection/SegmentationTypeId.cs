namespace OzonParserService.Domain.ConveerCollection;

public sealed class SegmentationTypeId : ValueObject
{
    public string Value { get; }
    private SegmentationTypeId(string value) => Value = value;
    public static SegmentationTypeId Create(string v) => new SegmentationTypeId(v);
    public static SegmentationTypeId CreateUnique() => new SegmentationTypeId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

public sealed class CollectionColorId : ValueObject
{
    public string Value { get; }
    private CollectionColorId(string value) => Value = value;
    public static CollectionColorId Create(string v) => new CollectionColorId(v);
    public static CollectionColorId CreateUnique() => new CollectionColorId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

public sealed class TrafficLightTypeId : ValueObject
{
    public string Value { get; }
    private TrafficLightTypeId(string value) => Value = value;
    public static TrafficLightTypeId Create(string v) => new TrafficLightTypeId(v);
    public static TrafficLightTypeId CreateUnique() => new TrafficLightTypeId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

public sealed class LoginomReqId : ValueObject
{
    public string Value { get; }
    private LoginomReqId(string value) => Value = value;
    public static LoginomReqId Create(string v) => new LoginomReqId(v);
    public static LoginomReqId CreateUnique() => new LoginomReqId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

public sealed class SegmentationId : ValueObject
{
    public string Value { get; }
    private SegmentationId(string value) => Value = value;
    public static SegmentationId Create(string v) => new SegmentationId(v);
    public static SegmentationId CreateUnique() => new SegmentationId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

public sealed class TrafficLightId : ValueObject
{
    public string Value { get; }
    private TrafficLightId(string value) => Value = value;
    public static TrafficLightId Create(string v) => new TrafficLightId(v);
    public static TrafficLightId CreateUnique() => new TrafficLightId(Guid.NewGuid().ToString());
    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}

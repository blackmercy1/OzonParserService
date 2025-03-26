namespace OzonParserService.Domain.Common.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();
    public bool Equals(ValueObject? other) => Equals((object?) other);
    public static bool operator ==(ValueObject left ,ValueObject right) => Equals(left, right);

    public static bool operator !=(ValueObject left, ValueObject right) => !Equals(left, right);

    public override int GetHashCode() => GetEqualityComponents()
        .Select(x => x.GetHashCode())
        .Aggregate(HashCode.Combine);
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;
        
        var valueObject = (ValueObject) obj;
        return GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());
    }
}

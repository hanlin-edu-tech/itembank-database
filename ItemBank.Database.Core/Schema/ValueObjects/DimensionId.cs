using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record DimensionId(string Value) : IConvertibleId<DimensionId, string>
{
    public string ToValue() => Value;
    public static DimensionId FromValue(string value) => new(value);
}

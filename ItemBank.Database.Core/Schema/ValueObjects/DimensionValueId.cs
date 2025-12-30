using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record DimensionValueId(string Value) : IConvertibleId<DimensionValueId, string>
{
    public string ToValue() => Value;
    public static DimensionValueId FromValue(string value) => new(value);
}

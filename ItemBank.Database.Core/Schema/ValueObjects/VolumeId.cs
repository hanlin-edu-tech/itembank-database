using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record VolumeId(string Value) : IConvertibleId<VolumeId, string>
{
    public string ToValue() => Value;
    public static VolumeId FromValue(string value) => new(value);
}

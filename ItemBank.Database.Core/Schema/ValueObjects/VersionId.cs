using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record VersionId(string Value) : IConvertibleId<VersionId, string>
{
    public string ToValue() => Value;
    public static VersionId FromValue(string value) => new(value);
}

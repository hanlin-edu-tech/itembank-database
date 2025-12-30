using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record SourceId(string Value) : IConvertibleId<SourceId, string>
{
    public string ToValue() => Value;
    public static SourceId FromValue(string value) => new(value);
}

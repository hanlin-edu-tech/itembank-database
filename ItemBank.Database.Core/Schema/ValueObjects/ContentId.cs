using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ContentId(string Value) : IConvertibleId<ContentId, string>
{
    public string ToValue() => Value;
    public static ContentId FromValue(string value) => new(value);
}

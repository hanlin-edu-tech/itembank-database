using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ContentSectionId(string Value) : IConvertibleId<ContentSectionId, string>
{
    public string ToValue() => Value;
    public static ContentSectionId FromValue(string value) => new(value);
}

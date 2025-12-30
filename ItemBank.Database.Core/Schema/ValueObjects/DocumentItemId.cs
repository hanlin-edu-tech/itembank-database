using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record DocumentItemId(string Value) : IConvertibleId<DocumentItemId, string>
{
    public string ToValue() => Value;
    public static DocumentItemId FromValue(string value) => new(value);
}

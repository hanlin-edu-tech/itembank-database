using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record DocumentId(string Value) : IConvertibleId<DocumentId, string>
{
    public string ToValue() => Value;
    public static DocumentId FromValue(string value) => new(value);
}

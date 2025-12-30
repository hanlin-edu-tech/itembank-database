using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record TextbookContentId(string Value) : IConvertibleId<TextbookContentId, string>
{
    public string ToValue() => Value;
    public static TextbookContentId FromValue(string value) => new(value);
}

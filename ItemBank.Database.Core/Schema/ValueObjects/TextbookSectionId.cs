using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record TextbookSectionId(string Value) : IConvertibleId<TextbookSectionId, string>
{
    public string ToValue() => Value;
    public static TextbookSectionId FromValue(string value) => new(value);
}

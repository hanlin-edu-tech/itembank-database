using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ItemId(string Value) : IConvertibleId<ItemId, string>
{
    public string ToValue() => Value;
    public static ItemId FromValue(string value) => new(value);
}

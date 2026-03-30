using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record ItemShellId(string Value) : IConvertibleId<ItemShellId, string>
{
    public string ToValue() => Value;
    public static ItemShellId FromValue(string value) => new(value);
}

using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record UserId(string Value) : IConvertibleId<UserId, string>
{
    public string ToValue() => Value;
    public static UserId FromValue(string value) => new(value);
}

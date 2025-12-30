using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

public record UserTypeId(string Value) : IConvertibleId<UserTypeId, string>
{
    public string ToValue() => Value;
    public static UserTypeId FromValue(string value) => new(value);
}

using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 使用者類型值 Id
/// </summary>
public record UserTypeValueId(string Value) : IConvertibleId<UserTypeValueId, string>
{
    public string ToValue() => Value;
    public static UserTypeValueId FromValue(string value) => new(value);
}

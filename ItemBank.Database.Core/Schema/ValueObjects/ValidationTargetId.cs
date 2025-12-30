using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 驗證目標 Id
/// </summary>
public record ValidationTargetId(string Value) : IConvertibleId<ValidationTargetId, string>
{
    public string ToValue() => Value;
    public static ValidationTargetId FromValue(string value) => new(value);
}

using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 難度 Id
/// </summary>
public record DifficultyId(string Value) : IConvertibleId<DifficultyId, string>
{
    public string ToValue() => Value;
    public static DifficultyId FromValue(string value) => new(value);
}

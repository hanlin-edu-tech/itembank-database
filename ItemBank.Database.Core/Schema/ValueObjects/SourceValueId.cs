using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 出處值 Id
/// </summary>
public record SourceValueId(string Value) : IConvertibleId<SourceValueId, string>
{
    public string ToValue() => Value;
    public static SourceValueId FromValue(string value) => new(value);
}

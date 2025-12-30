using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 題目問題 Id
/// </summary>
public record ItemIssueId(string Value) : IConvertibleId<ItemIssueId, string>
{
    public string ToValue() => Value;
    public static ItemIssueId FromValue(string value) => new(value);
}

using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 五欄檔案儲存庫 Id
/// </summary>
public record DocumentRepoId(string Value) : IConvertibleId<DocumentRepoId, string>
{
    public string ToValue() => Value;
    public static DocumentRepoId FromValue(string value) => new(value);
}

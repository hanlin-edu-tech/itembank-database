using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 目錄群組 Id
/// </summary>
public record CatalogGroupId(string Value) : IConvertibleId<CatalogGroupId, string>
{
    public string ToValue() => Value;
    public static CatalogGroupId FromValue(string value) => new(value);
}

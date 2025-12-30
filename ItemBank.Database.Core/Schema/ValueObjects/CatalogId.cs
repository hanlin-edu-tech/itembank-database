using ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

namespace ItemBank.Database.Core.Schema.ValueObjects;

/// <summary>
/// 目錄 Id
/// </summary>
public record CatalogId(string Value) : IConvertibleId<CatalogId, string>
{
    public string ToValue() => Value;
    public static CatalogId FromValue(string value) => new(value);
}

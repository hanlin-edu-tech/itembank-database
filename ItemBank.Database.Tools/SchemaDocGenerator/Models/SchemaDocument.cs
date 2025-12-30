namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

/// <summary>
/// Schema 文件模型（包含全局 Enum 定義和集合 Schema）
/// </summary>
public sealed record SchemaDocument
{
    /// <summary>全局 Enum 定義（key: Enum 名稱, value: name -> value 對應）</summary>
    public required IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Enums { get; init; }

    /// <summary>集合 Schema 清單</summary>
    public required IReadOnlyList<CollectionSchema> Collections { get; init; }
}

namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

/// <summary>
/// 集合 Schema 資訊模型
/// </summary>
public sealed record CollectionSchema
{
    /// <summary>MongoDB 集合名稱</summary>
    public required string CollectionName { get; init; }

    /// <summary>集合描述</summary>
    public required string Description { get; init; }

    /// <summary>集合建模種類</summary>
    public required string Kind { get; init; }

    /// <summary>判別欄位</summary>
    public string? Discriminator { get; init; }

    /// <summary>變動欄位</summary>
    public string? VariantField { get; init; }

    /// <summary>額外說明</summary>
    public required IReadOnlyList<string> Notes { get; init; }

    /// <summary>已建模的變體清單</summary>
    public required IReadOnlyList<CollectionVariantSchema> Variants { get; init; }

    /// <summary>型別名稱</summary>
    public required string TypeName { get; init; }

    /// <summary>是否實作 IAuditable 介面</summary>
    public required bool IsAuditable { get; init; }

    /// <summary>是否實作 IFinalizable 介面</summary>
    public required bool IsFinalizable { get; init; }

    /// <summary>索引清單</summary>
    public required IReadOnlyList<IndexSchema> Indices { get; init; }

    /// <summary>欄位定義（key 為欄位名稱）</summary>
    public required IReadOnlyDictionary<string, FieldSchema> Fields { get; init; }
}

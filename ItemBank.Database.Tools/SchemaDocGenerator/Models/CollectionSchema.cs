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

    /// <summary>C# 類別名稱</summary>
    public required string ClrTypeName { get; init; }

    /// <summary>是否實作 IAuditable 介面</summary>
    public required bool IsAuditable { get; init; }

    /// <summary>是否實作 IFinalizable 介面</summary>
    public required bool IsFinalizable { get; init; }

    /// <summary>索引清單</summary>
    public required IReadOnlyList<IndexSchema> Indices { get; init; }

    /// <summary>欄位定義（key 為欄位名稱）</summary>
    public required IReadOnlyDictionary<string, FieldSchema> Fields { get; init; }
}

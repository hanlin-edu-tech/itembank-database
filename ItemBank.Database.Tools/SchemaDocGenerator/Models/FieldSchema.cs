namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

/// <summary>
/// 欄位 Schema 資訊模型
/// </summary>
public sealed record FieldSchema
{
    /// <summary>
    /// MongoDB 型別字串
    /// 範例: "string", "number", "boolean", "datetime", "objectId", "array&lt;string&gt;", "array&lt;object&gt;", "object"
    /// </summary>
    public required string Type { get; init; }

    /// <summary>欄位描述</summary>
    public required string Description { get; init; }

    /// <summary>
    /// ValueObject 型別名稱（僅當欄位為 ValueObject 時使用）
    /// 例如: "ItemId", "UserId"
    /// </summary>
    public string? IdType { get; init; }

    /// <summary>
    /// Enum 型別名稱（僅當欄位為 Enum 時使用）
    /// 例如: "DimensionType", "ImportItemStatus"
    /// </summary>
    public string? EnumType { get; init; }

    /// <summary>
    /// 嵌套欄位定義（僅當 Type 為 "object" 或 "array&lt;object&gt;" 時使用）
    /// key 為欄位名稱
    /// </summary>
    public IReadOnlyDictionary<string, FieldSchema>? Fields { get; init; }

    /// <summary>
    /// Enum 的所有可能值（name -> value 對應，僅用於內部收集，不輸出到文件）
    /// </summary>
    public IReadOnlyDictionary<string, string>? EnumValues { get; init; }
}

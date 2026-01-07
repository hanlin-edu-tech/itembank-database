namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

/// <summary>
/// 索引 Schema 資訊模型
/// </summary>
public sealed record IndexSchema
{
    /// <summary>索引名稱</summary>
    public required string Name { get; init; }

    /// <summary>索引欄位清單</summary>
    public required IReadOnlyList<IndexField> Fields { get; init; }
}

/// <summary>
/// 索引欄位資訊
/// </summary>
public sealed record IndexField
{
    /// <summary>欄位名稱</summary>
    public required string FieldName { get; init; }

    /// <summary>排序方向 (ascending/descending)</summary>
    public required string Direction { get; init; }
}

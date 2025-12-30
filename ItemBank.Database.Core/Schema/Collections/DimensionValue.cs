using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DimensionValues")]
[Description("向度資訊")]
public class DimensionValue : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required DimensionValueId Id { get; init; }

    [Description("向度資訊表 Id")]
    public required string DimensionId { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("路徑")]
    public required List<DimensionValueId> Path { get; init; }

    [Description("作者")]
    public required UserId Author { get; init; }

    [Description("備註")]
    public required string Description { get; init; }

    [Description("說明")]
    public required string Explanation { get; init; }

    [Description("選文")]
    public required string Article { get; init; }

    [Description("摘要")]
    public required string Synopsis { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("深度")]
    public required int Depth { get; init; }

    [Description("上層 Id")]
    public required DimensionValueId? ParentId { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
    
    [Obsolete("節點的類型，棄用是因為在設計上有問題，會出現同時是根節點以及同時是葉節點的情況，建議改為用 Path 是否為空來判斷是否為根節點，用是否有子節點來判斷是否為葉節點")]
    [BsonIgnoreIfNull]
    public string? NodeType { get; init; }

    [Obsolete("過去用於版本追蹤，不確定設計方式")] 
    [BsonIgnoreIfNull]
    public string? LastVersionedName { get; init; }
    
    [Obsolete("過去使用軟刪除機制，現在沒有軟刪除")]
    [BsonIgnoreIfNull]
    public bool? Archived { get; init; }
}

using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemMergeHistories")]
[Description("題目合併歷史")]
public class ItemMergeHistory : IIndexable<ItemMergeHistory>
{
    [BsonId]
    [Description("Id")]
    public required ItemMergeHistoryId Id { get; init; }

    [Description("基準題 Id")]
    public required ItemId BaseItemId { get; init; }

    [Description("整併題 Id")]
    public required ItemId MergeItemId { get; init; }

    [Description("使用者 Id")]
    public required UserId UserId { get; init; }

    [Description("合併時間")]
    public required DateTime MergedAt { get; init; }

    [Description("合併原因")]
    public string? Reason { get; init; }

    [Description("基準題原始元資料")]
    public required Dictionary<string, object?> OriginalBaseMetadata { get; init; }

    [Description("整併題原始元資料")]
    public required Dictionary<string, object?> OriginalMergeMetadata { get; init; }

    [Description("合併操作清單")]
    public required IReadOnlyList<FieldMergeOperation> MergeOperations { get; init; }

    [Description("重複偵測記錄 Id")]
    public DuplicateDetectionRecordId? DuplicateDetectionRecordId { get; init; }

    [Description("是否可復原")]
    public required bool CanRevert { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }
}

[Description("欄位合併操作")]
public class FieldMergeOperation
{
    [Description("欄位名稱")]
    public required string FieldName { get; init; }

    [Description("欄位顯示名稱")]
    public required string FieldDisplayName { get; init; }

    [Description("原始值")]
    public object? OriginalValue { get; init; }

    [Description("合併值")]
    public object? MergeValue { get; init; }

    [Description("結果值")]
    public object? ResultValue { get; init; }

    [Description("原始顯示值")]
    public string? OriginalDisplayValue { get; init; }

    [Description("合併顯示值")]
    public string? MergeDisplayValue { get; init; }

    [Description("結果顯示值")]
    public string? ResultDisplayValue { get; init; }

    [Description("合併規則")]
    public required string MergeRule { get; init; }

    [Description("操作時間")]
    public required DateTime OperationTime { get; init; }

    [Description("備註")]
    public string? Notes { get; init; }
}

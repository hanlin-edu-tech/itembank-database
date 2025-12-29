using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DuplicateDetectionRecords")]
[Description("重複偵測記錄")]
public class DuplicateDetectionRecord
{
    [BsonId]
    [Description("Id")]
    public required DuplicateDetectionRecordId Id { get; init; }

    [Description("來源題目 Id")]
    public required ItemId SourceItemId { get; init; }

    [Description("目標題目 Id")]
    public required ItemId TargetItemId { get; init; }

    [Description("向量相似度分數")]
    public float? VectorSimilarity { get; init; }

    [Description("Gemini AI 判斷信心度")]
    public float? GeminiConfidence { get; init; }

    [Description("Gemini 判斷理由")]
    public string? GeminiReasoning { get; init; }

    [Description("使用的模型版本")]
    public string? ModelVersion { get; init; }

    [Description("偵測時間")]
    public required DateTime DetectedAt { get; init; }

    [Description("最後更新時間")]
    public required DateTime UpdatedAt { get; init; }

    [Description("錯誤訊息")]
    public string? ErrorMessage { get; init; }

    [Description("批次 Id")]
    public string? BatchId { get; init; }

    [Description("標記資訊")]
    public DuplicateMarkingInfo? MarkingInfo { get; init; }
}

[Description("重複標記資訊")]
public class DuplicateMarkingInfo
{
    [Description("是否標記為非重複題")]
    public bool? IsMarkedAsNonDuplicate { get; init; }

    [Description("標記者使用者 Id")]
    public UserId? MarkedByUserId { get; init; }

    [Description("標記時間")]
    public DateTime? MarkedAt { get; init; }

    [Description("標記原因")]
    public string? Reason { get; init; }
}

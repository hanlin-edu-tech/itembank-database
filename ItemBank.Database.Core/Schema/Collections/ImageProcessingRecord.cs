using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ImageProcessingRecords")]
[Description("圖片處理記錄")]
public class ImageProcessingRecord
{
    [BsonId]
    [Description("內容 SHA512 雜湊值")]
    public required string ContentSha512Hash { get; init; }

    [Description("嵌入向量")]
    public required IReadOnlyList<float> Embedding { get; init; }

    [Description("嵌入模型名稱")]
    public required string EmbeddingModelName { get; init; }

    [Description("處理類型")]
    public ProcessingType? ProcessingType { get; init; }

    [Description("處理內容")]
    public string? ProcessingContent { get; init; }

    [Description("處理信心度")]
    public double? ProcessingConfidence { get; init; }

    [Description("處理思考過程")]
    public string? ProcessingThinking { get; init; }

    [Description("處理時間")]
    public DateTime? ProcessedAt { get; init; }

    [Description("處理模型版本")]
    public string? ProcessingModelVersion { get; init; }

    [Description("批次 Id")]
    public string? BatchId { get; init; }

    [Description("批次狀態")]
    public string? BatchStatus { get; init; }

    [Description("批次提交時間")]
    public DateTime? BatchSubmittedAt { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("最後更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

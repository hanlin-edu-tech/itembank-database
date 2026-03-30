using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemImage.DuplicateMatches")]
[Description("題目圖片重複比對")]
public sealed class ItemImageDuplicateMatch : IIndexable<ItemImageDuplicateMatch>
{
[BsonId]
    [Description("Id")]
    public required ItemImageDuplicateMatchId Id { get; init; }

    [Description("批次 Id")]
    public required string BatchId { get; init; }

    [Description("偵測時間")]
    public required DateTime DetectedAt { get; init; }

    [Description("錯誤訊息")]
    public string? ErrorMessage { get; init; }

    [Description("Gemini 信心分數")]
    public double? GeminiConfidence { get; init; }

    [Description("Gemini 推理")]
    public required string GeminiReasoning { get; init; }

    [Description("題目 Id 清單")]
    public required IReadOnlyList<ItemId> ItemIds { get; init; }

    [Description("模型版本")]
    public string? ModelVersion { get; init; }

    [Description("來源題目 Id")]
    public required ItemId SourceItemId { get; init; }

    [Description("目標題目 Id")]
    public required ItemId TargetItemId { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    [Description("向量相似度")]
    public required double VectorSimilarity { get; init; }
}

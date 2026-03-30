using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using ItemBank.Database.Core.Schema.Enums;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemImage.ProcessingRecords")]
[Description("題目圖片處理紀錄")]
public sealed class ItemImageProcessingRecord : IIndexable<ItemImageProcessingRecord>
{
[BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("批次 Id")]
    public required string BatchId { get; init; }

    [Description("批次狀態")]
    public string? BatchStatus { get; init; }

    [Description("批次 Submitted時間")]
    public required DateTime BatchSubmittedAt { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("嵌入清單")]
    public required IReadOnlyList<double> Embedding { get; init; }

    [Description("嵌入嘗試次數")]
    public required int EmbeddingAttempts { get; init; }

    [Description("嵌入模型名稱")]
    public required string EmbeddingModelName { get; init; }

    [Description("嵌入狀態")]
    public required ItemImageJobStatus EmbeddingStatus { get; init; }

    [Description("Gemini 嘗試次數")]
    public required int GeminiAttempts { get; init; }

    [Description("Gemini 批次 Id")]
    public required string GeminiBatchId { get; init; }

    [Description("Gemini 狀態")]
    public required ItemImageJobStatus GeminiStatus { get; init; }

    [Description("圖片 URL")]
    public required string ImageUrl { get; init; }

    [Description("最後嵌入嘗試時間")]
    public required DateTime LastEmbeddingAttemptAt { get; init; }

    [Description("最後 Gemini 嘗試時間")]
    public required DateTime LastGeminiAttemptAt { get; init; }

    [Description("處理時間")]
    public required DateTime ProcessedAt { get; init; }

    [Description("處理 Confidence")]
    public required double ProcessingConfidence { get; init; }

    [Description("處理內容")]
    public string? ProcessingContent { get; init; }

    [Description("處理 ModelVersion")]
    public required string ProcessingModelVersion { get; init; }

    [Description("處理 Thinking")]
    public required string ProcessingThinking { get; init; }

    [Description("處理 類型")]
    public required ProcessingType ProcessingType { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    static IReadOnlyList<CreateIndexModel<ItemImageProcessingRecord>> IIndexable<ItemImageProcessingRecord>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Descending("createdAt"),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "createdAt_-1"
                }
        ),
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("embeddingLockId"),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "embeddingLockId_1"
                }
        ),
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Combine(
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("embeddingStatus"),
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("embeddingAttempts"),
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("batchSubmittedAt")
                ),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "embeddingStatus_1_embeddingAttempts_1_batchSubmittedAt_1"
                }
        ),
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("geminiBatchId"),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "geminiBatchId_1"
                }
        ),
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Combine(
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("geminiStatus"),
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("geminiAttempts"),
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("batchSubmittedAt")
                ),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "geminiStatus_1_geminiAttempts_1_batchSubmittedAt_1"
                }
        ),
        new(
            Builders<ItemImageProcessingRecord>.IndexKeys.Combine(
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("geminiStatus"),
                    Builders<ItemImageProcessingRecord>.IndexKeys.Ascending("geminiBatchId")
                ),
                new CreateIndexOptions<ItemImageProcessingRecord>
                {
                    Name = "geminiStatus_1_geminiBatchId_1"
                }
        )
    ];
}

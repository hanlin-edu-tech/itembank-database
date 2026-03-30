using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.GeneratedImages")]
[Description("AI 生成圖片")]
public sealed class AIGenerationGeneratedImage : IIndexable<AIGenerationGeneratedImage>
{
[BsonId]
    [Description("Id")]
    public required GeneratedImageId Id { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("描述")]
    public required string Description { get; init; }

    [Description("GCS 路徑")]
    public string? GcsPath { get; init; }

    [Description("生成題目 Id")]
    public required GeneratedItemId GeneratedItemId { get; init; }

    [Description("圖片類型")]
    public required GeneratedImageType ImageType { get; init; }

    [Description("是否啟用")]
    public required bool IsActive { get; init; }

    [Description("LLM 模型")]
    public required string LlmModel { get; init; }

    [Description("公開 URL")]
    public required string PublicUrl { get; init; }

    [Description("S3 路徑")]
    public string? S3Path { get; init; }

    [Description("版本")]
    public required int Version { get; init; }
}

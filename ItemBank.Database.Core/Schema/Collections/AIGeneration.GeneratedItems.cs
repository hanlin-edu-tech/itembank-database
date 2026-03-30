using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.GeneratedItems")]
[CollectionSchemaKind(CollectionSchemaKind.FlexibleCollection)]
[CollectionSchemaNote("答案與詳解結構可能依題型變化")]
[Description("AI 生成題目")]
public sealed class AIGenerationGeneratedItem : IIndexable<AIGenerationGeneratedItem>
{
[BsonId]
    [Description("Id")]
    public required GeneratedItemId Id { get; init; }

    [Description("答案數量")]
    public int? AnswerCount { get; init; }

    [Description("內容")]
    public required AIGenerationGeneratedItemContent Content { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("生成 參數")]
    public required AIGenerationGeneratedItemGenerationParams GenerationParams { get; init; }

    [Description("圖片 Id 清單")]
    public required IReadOnlyList<GeneratedImageId> ImageIds { get; init; }

    [Description("題型")]
    public required string ItemType { get; init; }

    [Description("LLM 模型")]
    public required string LlmModel { get; init; }

    [Description("來源文件 Id")]
    public required DocumentId SourceDocumentId { get; init; }

    [Description("來源題目 Id")]
    public required ItemId SourceItemId { get; init; }

    [Description("狀態")]
    public required GeneratedItemStatus Status { get; init; }

    [Description("科目 Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    [Description("版本")]
    public required int Version { get; init; }

    [Description("Word 匯出錯誤")]
    public string? WordExportError { get; init; }

    [Description("Word 匯出狀態")]
    public WordExportStatus? WordExportStatus { get; init; }

    [Description("Word 匯出 URL")]
    public string? WordExportUrl { get; init; }
}

[Description("內容")]
public sealed class AIGenerationGeneratedItemContent
{
    [Description("前言")]
    public IReadOnlyList<BsonValue>? Preamble { get; init; }

    [Description("題目清單")]
    public required IReadOnlyList<AIGenerationGeneratedItemContentQuestion> Questions { get; init; }

    [Description("詳解")]
    public required string Solution { get; init; }
}

[Description("內容題目")]
public sealed class AIGenerationGeneratedItemContentQuestion
{
    [Description("答案")]
    public required BsonValue Answers { get; init; }

    [Description("選項清單")]
    public IReadOnlyList<string>? Options { get; init; }

    [Description("詳解")]
    public IReadOnlyList<BsonValue>? Solution { get; init; }

    [Description("題幹")]
    public required string Stem { get; init; }
}

[Description("生成 參數")]
public sealed class AIGenerationGeneratedItemGenerationParams
{
    [Description("認知層級")]
    public required string CognitiveLevel { get; init; }

    [Description("情境類型")]
    public required string ContextType { get; init; }

    [Description("難度")]
    public required int Difficulty { get; init; }

    [Description("知識點")]
    public required string KnowledgePoint { get; init; }
}

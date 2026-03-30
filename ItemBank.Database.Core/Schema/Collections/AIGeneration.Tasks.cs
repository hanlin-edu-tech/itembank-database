using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.Tasks")]
[Description("AI 生成任務")]
public sealed class AIGenerationTask : IIndexable<AIGenerationTask>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("完成時間")]
    public DateTime? CompletedAt { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("文件 Id")]
    public DocumentId? DocumentId { get; init; }

    [Description("文件儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }

    [Description("錯誤訊息")]
    public string? Error { get; init; }

    [Description("最大重試次數")]
    public required int MaxRetries { get; init; }

    [Description("參數")]
    public required AIGenerationTaskParams Params { get; init; }

    [Description("優先度")]
    public required int Priority { get; init; }

    [Description("請求者 Id")]
    public UserId? RequesterId { get; init; }

    [Description("結果")]
    public AIGenerationTaskResult? Result { get; init; }

    [Description("重試次數")]
    public required int RetryCount { get; init; }

    [Description("來源清單")]
    public required IReadOnlyList<AIGenerationTaskSource> Sources { get; init; }

    [Description("開始時間")]
    public required DateTime StartedAt { get; init; }

    [Description("狀態")]
    public required AIGenerationTaskStatus Status { get; init; }

    [Description("任務類型")]
    public required string TaskType { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

[Description("參數")]
public sealed class AIGenerationTaskParams
{
    [Description("額外指示")]
    public string? AdditionalInstructions { get; init; }

    [Description("模型")]
    public string? Model { get; init; }

    [Description("提示詞 Id")]
    public PromptId? PromptId { get; init; }
}

[Description("結果")]
public sealed class AIGenerationTaskResult
{
    [Description("失敗數量")]
    public required int FailedCount { get; init; }

    [Description("生成題目 Id 清單")]
    public required IReadOnlyList<GeneratedItemId> GeneratedItemIds { get; init; }

    [Description("處理時間毫秒")]
    public required int ProcessingTimeMs { get; init; }

    [Description("成功數量")]
    public required int SuccessCount { get; init; }
}

[Description("來源")]
public sealed class AIGenerationTaskSource
{
    [Description("數量")]
    public required int Count { get; init; }

    [Description("難度清單")]
    public IReadOnlyList<BsonValue>? Difficulty { get; init; }

    [Description("題目 Id")]
    public ItemId? ItemId { get; init; }

    [Description("章節 Id")]
    public ProductSectionId? SectionId { get; init; }

    [Description("題殼 Id")]
    public required ItemShellId ShellId { get; init; }
}

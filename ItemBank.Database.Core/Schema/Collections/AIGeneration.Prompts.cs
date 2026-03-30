using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.Prompts")]
[Description("AI 生成提示詞")]
public sealed class AIGenerationPrompt : IIndexable<AIGenerationPrompt>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("內容")]
    public required string Content { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("編輯者")]
    public required string EditedBy { get; init; }

    [Description("展開內容")]
    public string? ExpandedContent { get; init; }

    [Description("包含清單")]
    public IReadOnlyList<AIGenerationPromptInclude>? Includes { get; init; }

    [Description("是否為預設")]
    public required bool IsDefault { get; init; }

    [Description("提示詞 Id")]
    public required PromptId PromptId { get; init; }

    [Description("類型")]
    public required AIGenerationPromptType Type { get; init; }

    [Description("版本")]
    public required int Version { get; init; }

    static IReadOnlyList<CreateIndexModel<AIGenerationPrompt>> IIndexable<AIGenerationPrompt>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<AIGenerationPrompt>.IndexKeys.Ascending("promptId"),
                new CreateIndexOptions<AIGenerationPrompt>
                {
                    Name = "idx_promptId_unique_default",
                    Unique = true,
                    PartialFilterExpression = new BsonDocument { { "isDefault", true } }
                }
        ),
        new(
            Builders<AIGenerationPrompt>.IndexKeys.Combine(
                    Builders<AIGenerationPrompt>.IndexKeys.Ascending("promptId"),
                    Builders<AIGenerationPrompt>.IndexKeys.Descending("version")
                ),
                new CreateIndexOptions<AIGenerationPrompt>
                {
                    Name = "idx_promptId_version"
                }
        )
    ];
}

[Description("包含條件")]
public sealed class AIGenerationPromptInclude
{
    [Description("作為清單")]
    public required IReadOnlyList<string> As { get; init; }

    [Description("是否作為完整內容")]
    public required bool AsWholeContent { get; init; }

    [Description("條件")]
    public required string Condition { get; init; }

    [Description("路徑")]
    public required string Path { get; init; }
}

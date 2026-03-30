using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Prompts_bak")]
[Description("提示詞 bak")]
public sealed class PromptsBak : IIndexable<PromptsBak>
{
[BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("包含清單")]
    public IReadOnlyList<PromptsBakInclude>? Includes { get; init; }

    [Description("提示詞")]
    public string? Prompt { get; init; }

    [Description("原始")]
    public string? Raw { get; init; }

    [Description("類型")]
    public required string Type { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

[Description("包含條件")]
public sealed class PromptsBakInclude
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

using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AIGeneration.PromptCounters")]
[Description("AI 生成提示詞計數")]
public sealed class AIGenerationPromptCounter : IIndexable<AIGenerationPromptCounter>
{
[BsonId]
    [Description("Id")]
    public required PromptId Id { get; init; }

    [Description("序號")]
    public required int Seq { get; init; }
}

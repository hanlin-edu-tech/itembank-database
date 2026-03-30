using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.Documents")]
[Description("聊天聊天文件")]
public sealed class ChatBotDocument : IIndexable<ChatBotDocument>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("內容")]
    public required string Content { get; init; }

    [Description("文件 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("文件 Name")]
    public required string DocumentName { get; init; }

    [Description("嵌入清單")]
    public required IReadOnlyList<double> Embedding { get; init; }

    [Description("中繼資料")]
    public IReadOnlyList<BsonValue>? Metadata { get; init; }

    [Description("狀態")]
    public required string Status { get; init; }

    [Description("標題")]
    public required string Title { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    [Description("URL")]
    public string? Url { get; init; }
}

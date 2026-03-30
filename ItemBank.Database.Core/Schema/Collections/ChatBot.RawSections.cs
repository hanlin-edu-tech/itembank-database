using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.RawSections")]
[Description("聊天聊天原始章節")]
public sealed class ChatBotRawSection : IIndexable<ChatBotRawSection>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("認領時間")]
    public IReadOnlyList<BsonValue>? ClaimedAt { get; init; }

    [Description("認領者")]
    public IReadOnlyList<BsonValue>? ClaimedBy { get; init; }

    [Description("內容")]
    public required string Content { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("文件 Name")]
    public required string DocumentName { get; init; }

    [Description("重試次數")]
    public int? RetryCount { get; init; }

    [Description("章節 Index")]
    public required int SectionIndex { get; init; }

    [Description("狀態")]
    public string? Status { get; init; }

    [Description("類型")]
    public string? Type { get; init; }

    [Description("上傳者")]
    public string? UploadedBy { get; init; }

    [Description("版本")]
    public int? Version { get; init; }

    static IReadOnlyList<CreateIndexModel<ChatBotRawSection>> IIndexable<ChatBotRawSection>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ChatBotRawSection>.IndexKeys.Combine(
                    Builders<ChatBotRawSection>.IndexKeys.Ascending("documentName"),
                    Builders<ChatBotRawSection>.IndexKeys.Ascending("status"),
                    Builders<ChatBotRawSection>.IndexKeys.Ascending("claimedAt")
                ),
                new CreateIndexOptions<ChatBotRawSection>
                {
                    Name = "documentName_1_status_1_claimedAt_1"
                }
        )
    ];
}

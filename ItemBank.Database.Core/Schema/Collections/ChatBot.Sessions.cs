using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using ItemBank.Database.Core.Schema.Enums;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.Sessions")]
[CollectionSchemaKind(CollectionSchemaKind.FlexibleCollection)]
[CollectionSchemaNote("聊天訊息內容依訊息種類切換形狀")]
[CollectionSchemaNote("附加屬性與註記欄位保留動態型別，以反映自由格式資料")]
[Description("聊天 Session")]
public sealed class ChatBotSession : IIndexable<ChatBotSession>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("聊天 Data")]
    public ChatBotSessionChatData? ChatData { get; init; }

    [Description("聊天 MessagesJson")]
    public string? ChatMessagesJson { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("模型 Id")]
    public string? ModelId { get; init; }

    [Description("Session Id")]
    public required string SessionId { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }

    static IReadOnlyList<CreateIndexModel<ChatBotSession>> IIndexable<ChatBotSession>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ChatBotSession>.IndexKeys.Descending("createdAt"),
                new CreateIndexOptions<ChatBotSession>
                {
                    Name = "createdAt_-1"
                }
        ),
        new(
            Builders<ChatBotSession>.IndexKeys.Combine(
                    Builders<ChatBotSession>.IndexKeys.Ascending("modelId"),
                    Builders<ChatBotSession>.IndexKeys.Descending("createdAt")
                ),
                new CreateIndexOptions<ChatBotSession>
                {
                    Name = "modelId_1_createdAt_-1"
                }
        ),
        new(
            Builders<ChatBotSession>.IndexKeys.Ascending("sessionId"),
                new CreateIndexOptions<ChatBotSession>
                {
                    Name = "sessionId_1",
                    Unique = true
                }
        ),
        new(
            Builders<ChatBotSession>.IndexKeys.Descending("updatedAt"),
                new CreateIndexOptions<ChatBotSession>
                {
                    Name = "updatedAt_-1"
                }
        ),
        new(
            Builders<ChatBotSession>.IndexKeys.Combine(
                    Builders<ChatBotSession>.IndexKeys.Ascending("userId"),
                    Builders<ChatBotSession>.IndexKeys.Descending("createdAt")
                ),
                new CreateIndexOptions<ChatBotSession>
                {
                    Name = "userId_1_createdAt_-1"
                }
        )
    ];
}

[Description("聊天 Data")]
public sealed class ChatBotSessionChatData
{
    [Description("訊息清單")]
    public required IReadOnlyList<ChatBotSessionChatDataMessage> Messages { get; init; }

    [Description("模型 Id")]
    public required string ModelId { get; init; }
}

[Description("聊天 DataMessage")]
public sealed class ChatBotSessionChatDataMessage
{
    [Description("附加屬性")]
    public IReadOnlyList<BsonValue>? AdditionalProperties { get; init; }

    [Description("AuthorName")]
    public string? AuthorName { get; init; }

    [Description("內容清單")]
    public required IReadOnlyList<ChatBotSessionChatDataMessageContent> Contents { get; init; }

    [Description("建立時間")]
    public string? CreatedAt { get; init; }

    [Description("Message Id")]
    public string? MessageId { get; init; }

    [Description("Role")]
    public required ConversationRole Role { get; init; }
}

[Description("聊天 DataMessage 內容")]
public sealed class ChatBotSessionChatDataMessageContent
{
[BsonElement("$type")]
    [Description("類型")]
    public string? Type { get; init; }

    [Description("附加屬性")]
    public IReadOnlyList<BsonValue>? AdditionalProperties { get; init; }

    [Description("註記")]
    public IReadOnlyList<BsonValue>? Annotations { get; init; }

    [Description("Arguments")]
    public ChatBotSessionChatDataMessageContentArguments? Arguments { get; init; }

    [Description("Call Id")]
    public string? CallId { get; init; }

    [Description("InformationalOnly")]
    public bool? InformationalOnly { get; init; }

    [Description("名稱")]
    public string? Name { get; init; }

    [Description("ProtectedData")]
    public string? ProtectedData { get; init; }

    [Description("結果")]
    public string? Result { get; init; }

    [Description("Text")]
    public string? Text { get; init; }

    [Description("Uri")]
    public string? Uri { get; init; }
}

[Description("聊天 DataMessage 內容 Arguments")]
public sealed class ChatBotSessionChatDataMessageContentArguments
{
    [Description("查詢")]
    public required string Query { get; init; }
}

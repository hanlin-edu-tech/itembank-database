using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.Images")]
[Description("聊天聊天圖片")]
public sealed class ChatBotImage : IIndexable<ChatBotImage>
{
[BsonId]
    [Description("Id")]
    public required ChatBotImageId Id { get; init; }

    [Description("資料")]
    public required byte[] Data { get; init; }

    [Description("媒體類型")]
    public required string MediaType { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

[Description("Id")]
public sealed class ChatBotImageId
{
    [Description("文件 Name")]
    public required string DocumentName { get; init; }

    [Description("圖片 Index")]
    public required int ImageIndex { get; init; }

    [Description("版本")]
    public required int Version { get; init; }
}

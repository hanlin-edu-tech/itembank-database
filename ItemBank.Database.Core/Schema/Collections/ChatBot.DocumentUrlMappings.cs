using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ChatBot.DocumentUrlMappings")]
[Description("聊天文件 URL 對應")]
public sealed class ChatBotDocumentUrlMapping : IIndexable<ChatBotDocumentUrlMapping>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("文件 Name")]
    public required string DocumentName { get; init; }

    [Description("URL")]
    public required string Url { get; init; }
}

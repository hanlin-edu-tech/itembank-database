using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Announcements")]
[Description("公告")]
public sealed class Announcement: IIndexable<Announcement>
{
    [BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("公告內容")]
    public required string Content { get; init; }

    [Description("創建者")]
    public required UserId Creator { get; init; }
    
    [Description("建立時間")]
    public required DateTime CreatedTime { get; init; }
    
    [Description("版本")]
    public required int Version { get; init; }
}
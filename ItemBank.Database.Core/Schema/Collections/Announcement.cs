using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("公告")]
public sealed class Announcement
{
    [BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }
    
    [Description("公告內容")]
    public required string Content { get; init; }
    
    [Description("創建者")]
    public required string Creator { get; init; }
    
    [Description("建立時間")]
    public required DateTime CreatedTime { get; init; }
    
    [Description("版本")]
    public required int Version { get; init; }
}
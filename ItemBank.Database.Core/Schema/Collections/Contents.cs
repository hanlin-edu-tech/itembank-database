using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Contents")]
[Description("內容")]
public sealed class Content : IIndexable<Content>
{
[BsonId]
    [Description("Id")]
    public required ContentId Id { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("定稿時間")]
    public DateTime? FinalizedOn { get; init; }

    [Description("是否定稿")]
    public required bool IsFinalized { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("修訂版次")]
    public required int Revision { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    [Description("版本 Id")]
    public required VersionId VersionId { get; init; }

    [Description("冊次 Id")]
    public required VolumeId VolumeId { get; init; }
}

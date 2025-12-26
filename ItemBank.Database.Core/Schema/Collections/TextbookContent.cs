using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("課本章節表")]
public class TextbookContent : IFinalizable, IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("冊次 Id")]
    public required string VolumeId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("版本 Id")]
    public required string VersionId { get; init; }

    [Description("是否已鎖定")]
    public required bool IsFinalized { get; init; }

    [Description("版本")]
    public required int Revision { get; init; }

    [Description("鎖定時間")]
    public DateTime? FinalizedOn { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

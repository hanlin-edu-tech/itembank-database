using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("向度資訊表")]
public sealed class Dimension : IFinalizable, IAuditable
{
    [BsonId]
    [Description("Id")]
    public required DimensionId Id { get; init; }

    [Description("類型")]
    public required string Type { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("科目 Id 清單")]
    public required List<SubjectId> SubjectIds { get; init; }

    [Description("是否已鎖定")]
    public bool IsFinalized { get; init; }

    [Description("版本")]
    public int Revision { get; init; }

    [Description("鎖定時間")]
    public DateTime? FinalizedOn { get; init; }

    [Description("版本號")]
    public required string Version { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}
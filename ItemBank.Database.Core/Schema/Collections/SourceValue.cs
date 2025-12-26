using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("出處")]
public sealed class SourceValue : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("出處表 Id")]
    public required string SourceId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("版權")]
    public required bool Copyright { get; init; }

    [Description("描述")]
    public required string Description { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

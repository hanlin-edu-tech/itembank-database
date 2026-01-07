using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("SourceValues")]
[Description("出處")]
public sealed class SourceValue : IAuditable, IIndexable<SourceValue>
{
    [BsonId]
    [Description("Id")]
    public required SourceValueId Id { get; init; }

    [Description("出處表 Id")]
    public required SourceId SourceId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("版權")]
    public required bool Copyright { get; init; }

    [Description("備註")]
    public required string Description { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
    
    [Obsolete("已棄用，過去用於追蹤版本的名稱")]
    [BsonIgnoreIfNull]
    public string? LastVersionedName { get; init; }
}

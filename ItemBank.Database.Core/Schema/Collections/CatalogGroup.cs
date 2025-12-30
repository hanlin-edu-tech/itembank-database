using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("CatalogGroups")]
[Description("產品")]
public sealed class CatalogGroup : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required CatalogGroupId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("備註")]
    public required string? Description { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

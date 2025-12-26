using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("產品")]
public sealed class CatalogGroup : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required CatalogGroupId Id { get; init; }

    [Description("代碼")]
    public string Code { get; init; } = null!;

    [Description("名稱")]
    public string Name { get; init; } = null!;

    [Description("描述")]
    public string? Description { get; init; }

    [Description("建立者")]
    public UserId CreatedBy { get; init; } = null!;

    [Description("建立時間")]
    public DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public UserId UpdatedBy { get; init; } = null!;

    [Description("更新時間")]
    public DateTime UpdatedOn { get; init; }
}

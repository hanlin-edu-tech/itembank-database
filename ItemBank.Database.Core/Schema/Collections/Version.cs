using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Versions")]
[Description("版本")]
public class Version : IIndexable<Version>
{
    [BsonId]
    [Description("Id")]
    public required VersionId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }
    
    [Description("排序索引")]
    public required int OrderIndex { get; init; }
}

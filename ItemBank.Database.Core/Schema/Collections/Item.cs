using System.ComponentModel;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("題目")]
public class Item
{
    [BsonId]
    [Description("Id")]
    public required ItemId Id { get; init; }

    [Description("資源連結清單")]
    public List<ItemResourceLink> ResourceLinks { get; init; } = [];
}

[Description("題目資源連結")]
public class ItemResourceLink
{
    [Description("名稱")]
    public required string Name { get; init; }

    [Description("超連結")]
    public required string Href { get; init; }
}

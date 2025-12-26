using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("項目")]
public class Item
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; set; }

    [Description("資源連結清單")]
    public List<ItemResourceLink> ResourceLinks { get; set; } = [];
}

[Description("項目資源連結")]
public class ItemResourceLink
{
    [Description("名稱")]
    public required string Name { get; set; }

    [Description("超連結")]
    public required string Href { get; set; }
}

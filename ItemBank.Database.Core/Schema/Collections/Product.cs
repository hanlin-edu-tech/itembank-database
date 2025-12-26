using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("產品")]
public sealed class Product
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("標籤清單")]
    public required List<string> Tags { get; init; }

    [Description("目的")]
    public required string Purpose { get; init; }
}

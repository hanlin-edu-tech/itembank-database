using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("Products")]
[Description("產品（產品單元表的產品）")]
public sealed class Product : IIndexable<Product>
{
    [BsonId]
    [Description("Id")]
    public required ProductId Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("標籤清單")]
    public required IReadOnlyList<string> Tags { get; init; }

    [Description("目的")]
    public required string Purpose { get; init; }
}

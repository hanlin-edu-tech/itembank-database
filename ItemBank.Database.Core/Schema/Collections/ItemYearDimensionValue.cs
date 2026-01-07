using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemYearDimensionValues")]
[Description("題目學年向度資訊")]
public class ItemYearDimensionValue : IIndexable<ItemYearDimensionValue>
{
    [BsonId]
    [Description("Id")]
    public required ItemYearDimensionValueId Id { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId BodyOfKnowledgeId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("題號索引")]
    public required int QuestionIndex { get; init; }

    [Description("向度資訊 Id")]
    public required DimensionValueId DimensionValueId { get; init; }

    [Description("使用類型")]
    public required UsageType UsageType { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    static IReadOnlyList<CreateIndexModel<ItemYearDimensionValue>> IIndexable<ItemYearDimensionValue>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ItemYearDimensionValue>.IndexKeys.Ascending(x => x.ItemId)
        ),
        new(
            Builders<ItemYearDimensionValue>.IndexKeys
                .Ascending(x => x.ItemId)
                .Ascending(x => x.Year)
                .Ascending(x => x.BodyOfKnowledgeId)
                .Ascending(x => x.DimensionValueId)
                .Ascending(x => x.OrderIndex)
                .Ascending(x => x.UsageType)
                .Ascending(x => x.QuestionIndex)
        ),
        new(
            Builders<ItemYearDimensionValue>.IndexKeys.Ascending(x => x.DimensionValueId)
        )
    ];
}

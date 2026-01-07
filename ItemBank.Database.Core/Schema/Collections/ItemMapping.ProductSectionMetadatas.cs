using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemMapping.ProductSectionMetadatas")]
[Description("產品單元章節元資料")]
public class ProductSectionMetadata : IIndexable<ProductSectionMetadata>
{
    [BsonId]
    [Description("產品單元章節 Id")]
    public required ProductSectionId Id { get; init; }

    [Description("產品單元 Id")]
    public required ProductContentId ProductContentId { get; init; }

    [Description("族譜產品單元 Id 清單")]
    public required IReadOnlyList<ProductContentId> PedigreeContentIds { get; init; }

    [Description("最大族譜向度資訊 Id 清單")]
    public required IReadOnlyList<DimensionValueId> MaxPedisgreeDimensionValueIds { get; init; }

    [Description("族譜向度資訊 Id 清單")]
    public required IReadOnlyList<DimensionValueId> PedisgreeDimensionValueIds { get; init; }

    [Description("向度資訊清單")]
    public required IReadOnlyList<ProductSectionDimValue> DimensionValues { get; init; }

    [Description("是否有課程")]
    public required bool HasLesson { get; init; }

    [Description("是否有知識點")]
    public required bool HasKnowledge { get; init; }

    [Description("排序")]
    public required int Order { get; init; }

    [Description("年度")]
    public required int Year { get; init; }

    static IReadOnlyList<CreateIndexModel<ProductSectionMetadata>> IIndexable<ProductSectionMetadata>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ProductSectionMetadata>.IndexKeys.Ascending(x => x.ProductContentId)
        ),
        new(
            Builders<ProductSectionMetadata>.IndexKeys.Ascending(x => x.PedigreeContentIds)
        )
    ];
}

[Description("產品單元章節向度資訊")]
public class ProductSectionDimValue
{
    [Description("向度資訊 Id")]
    public required DimensionValueId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }

    [Description("向度資訊表 Id")]
    public required DimensionId DimensionId { get; init; }
}

using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemYearDimensionValues")]
[Description("題目年份維度值")]
public class ItemYearDimensionValue
{
    [BsonId]
    [Description("Id")]
    public required ItemYearDimensionValueId Id { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId BodyOfKnowledgeId { get; init; }

    [Description("年度")]
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
}

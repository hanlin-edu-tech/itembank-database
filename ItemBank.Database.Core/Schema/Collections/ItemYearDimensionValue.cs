using System.ComponentModel;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("題目年份維度值")]
public class ItemYearDimensionValue
{
    [BsonId]
    [Description("Id")]
    public required ItemYearDimensionValueId Id { get; init; }

    [Description("向度資訊 Id")]
    public required string DimensionValueId { get; init; }

    [Description("題目 Id")]
    public required string ItemId { get; init; }
}

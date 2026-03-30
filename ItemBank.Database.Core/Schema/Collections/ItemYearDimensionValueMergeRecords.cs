using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ItemBank.Database.Core.Schema.ValueObjects;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemYearDimensionValueMergeRecords")]
[Description("題目年度向度值合併紀錄")]
public sealed class ItemYearDimensionValueMergeRecord : IIndexable<ItemYearDimensionValueMergeRecord>
{
[BsonId]
    [Description("Id")]
    public required ObjectId Id { get; init; }

    [Description("基礎 題目 Id")]
    public required ItemId BaseItemId { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId BodyOfKnowledgeId { get; init; }

    [Description("題目圖片重複比對 Id")]
    public required ItemImageDuplicateMatchId ItemImageDuplicateMatchId { get; init; }

    [Description("合併時間")]
    public required DateTime MergedAt { get; init; }

    [Description("合併By 使用者 Id")]
    public required UserId MergedByUserId { get; init; }

    [Description("合併 題目 Id")]
    public required ItemId MergedItemId { get; init; }

    [Description("年度")]
    public required int Year { get; init; }
}

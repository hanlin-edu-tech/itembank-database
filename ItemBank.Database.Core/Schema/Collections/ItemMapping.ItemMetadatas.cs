using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemMapping.ItemMetadatas")]
[Description("題目元資料")]
public class ItemMetadata : IIndexable<ItemMetadata>
{
    [BsonId]
    [Description("題目 Id")]
    public required ItemId Id { get; init; }

    [Description("作答方式")]
    public required IReadOnlyList<string> AnsweringMethods { get; init; }

    [Description("題數")]
    public required int QuestionCount { get; init; }

    [Description("是否線上")]
    public required bool IsOnline { get; init; }
}

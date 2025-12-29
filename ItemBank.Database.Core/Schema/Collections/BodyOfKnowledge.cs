using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("BodiesOfKnowledge")]
[Description("學程")]
public class BodyOfKnowledge
{
    [BsonId]
    [Description("Id")]
    public required BodyOfKnowledgeId Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("科目Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("終止學年")]
    public required int? FinalYear { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("向度資訊表的 Id 列表")]
    public required IReadOnlyList<DimensionId> DimensionIds { get; init; }
    
    [Obsolete("舊總庫(v1)有刪除功能，但現在沒有刪除功能，所以這個欄位沒用")]
    [BsonIgnoreIfNull]
    public bool? Archived { get; init; }
}

using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("學程")]
public class BodyOfKnowledge
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("科目Id")]
    public required string SubjectId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("終止學年")]
    public required int? FinalYear { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("向度資訊表的 Id 列表")]
    public required IEnumerable<string> DimensionIds { get; init; }
}

using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("科目")]
public sealed class Subject
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("父科目 Id")]
    public required string? ParentSubjectId { get; init; }

    [Description("教育代碼")]
    public required string EducationCode { get; init; }

    [Description("教育名稱")]
    public required string EducationName { get; init; }

    [Description("簡稱")]
    public required string SimpleName { get; init; }

    [Description("教育排序索引")]
    public required int EducationOrderIndex { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("領域")]
    public required string Area { get; init; }
}

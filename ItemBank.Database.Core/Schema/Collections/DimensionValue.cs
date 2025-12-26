using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("向度資訊")]
public class DimensionValue : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("向度資訊表 Id")]
    public required string DimensionId { get; init; }

    [Description("類型")]
    public required string Type { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("路徑")]
    public required List<string> Path { get; init; }

    [Description("作者")]
    public required string Author { get; init; }

    [Description("描述")]
    public required string Description { get; init; }

    [Description("說明")]
    public required string Explanation { get; init; }

    [Description("文章")]
    public required string Article { get; init; }

    [Description("摘要")]
    public required string Synopsis { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("深度")]
    public required int Depth { get; init; }

    [Description("上層 Id")]
    public required string? ParentId { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

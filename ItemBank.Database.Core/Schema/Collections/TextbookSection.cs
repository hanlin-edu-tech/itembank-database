using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("TextbookSections")]
[Description("課本章節")]
public class TextbookSection : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required TextbookSectionId Id { get; init; }

    [Description("冊次名稱")]
    public required string VolumeName { get; init; }

    [Description("課本章節表 Id")]
    public required TextbookContentId TextbookContentId { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("段考範圍")]
    public required List<string> TermTests { get; init; }

    [Description("備註")]
    public required string Description { get; init; }

    [Description("向度資訊 Id 清單")]
    public required List<DimensionValueId> DimensionValueIds { get; init; }

    [Description("路徑")]
    public required List<TextbookSectionId> Path { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

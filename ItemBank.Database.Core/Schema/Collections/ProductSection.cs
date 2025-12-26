using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("產品章節")]
public class ProductSection : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("產品內容 Id")]
    public required string ProductContentId { get; init; }

    [Description("冊別")]
    public required string Volume { get; init; }

    [Description("代碼")]
    public required string Code { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("段考範圍")]
    public required List<string> TermTests { get; init; }

    [Description("備註")]
    public required string Description { get; init; }

    [Description("五欄檔案 Id 清單")]
    public required List<string> DocumentIds { get; init; }

    [Description("向度資訊 Id 清單")]
    public required List<string> DimensionValueIds { get; init; }

    [Description("是否可見")]
    public required bool IsVisible { get; init; }

    [Description("是否啟用")]
    public required bool IsEnable { get; init; }

    [Description("是否可摺疊")]
    public required bool IsCollapsible { get; init; }

    [Description("路徑")]
    public required List<string> Path { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    [Description("是否有子項目")]
    public required bool HasChildren { get; init; }
}

using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DocumentItems")]
[Description("五欄檔案題目")]
public class DocumentItem
{
    [BsonId]
    [Description("Id")]
    public required DocumentItemId Id { get; init; }

    [Description("五欄檔案 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("排序")]
    public required int Order { get; init; }

    [Description("新增時間")]
    public required DateTime AddedOn { get; init; }

    [Description("題目文字")]
    public string? ItemText { get; init; }

    [Description("是否修改？不確定用途")]
    public required bool? Marked { get; init; }

    [Description("是否為新題目")]
    public required bool IsNewItem { get; init; }

    [Description("元資料清單，放置出處及題型")]
    public required IReadOnlyList<DocumentItemMetadata> MetadataList { get; init; }
    
    [Obsolete("元資料，發現註解上有標示應該可以棄用，暫時留著。應該是處理過程中會用到的資料")]
    public Dictionary<string, string>? Metadata { get; init; }

}

[Description("五欄檔案題目元資料")]
public class DocumentItemMetadata
{
    [Description("元資料值 Id")]
    public required string MetadataValueId { get; init; }

    [Description("元資料類型")]
    public required MetadataType MetadataType { get; init; }

    [Description("元資料值名稱")]
    public required string MetadataValueName { get; init; }
}

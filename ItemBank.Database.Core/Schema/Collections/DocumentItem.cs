using System.ComponentModel;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("五欄檔案題目")]
public class DocumentItem
{
    [BsonId]
    [Description("Id")]
    public required DocumentItemId Id { get; init; }

    [Description("五欄檔案 Id")]
    public required string DocumentId { get; init; }

    [Description("題目 Id")]
    public required string ItemId { get; init; }

    [Description("元資料清單")]
    public List<DocumentItemMetadata> MetadataList { get; init; } = [];
}

[Description("五欄檔案題目元資料")]
public class DocumentItemMetadata
{
    [Description("元資料值 Id")]
    public required string MetadataValueId { get; init; }

    [Description("元資料類型")]
    public required string MetadataType { get; init; }

    [Description("元資料值名稱")]
    public string? MetadataValueName { get; init; }
}

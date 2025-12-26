using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("文件項目")]
public class DocumentItem
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; set; }

    [Description("文件 Id")]
    public required string DocumentId { get; set; }

    [Description("項目 Id")]
    public required string ItemId { get; set; }

    [Description("元資料清單")]
    public List<DocumentItemMetadata> MetadataList { get; set; } = [];
}

[Description("文件項目元資料")]
public class DocumentItemMetadata
{
    [Description("元資料值 Id")]
    public required string MetadataValueId { get; set; }

    [Description("元資料類型")]
    public required string MetadataType { get; set; }

    [Description("元資料值名稱")]
    public string? MetadataValueName { get; set; }
}

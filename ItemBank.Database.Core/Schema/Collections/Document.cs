using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("五欄檔案")]
public class Document
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("五欄檔案儲存庫 Id")]
    public required string DocumentRepoId { get; init; }

    [Description("產品章節 Id 清單")]
    public required List<string> ProductSectionIds { get; init; }
}

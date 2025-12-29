using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemIssues")]
[Description("題目問題")]
public class ItemIssue
{
    [BsonId]
    [Description("Id")]
    public required ItemIssueId Id { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("五欄檔案 Id")]
    public DocumentId? DocumentId { get; init; }

    [Description("檔案名稱")]
    public string? FileName { get; init; }

    [Description("標題")]
    public required string Title { get; init; }

    [Description("標籤清單")]
    public required IReadOnlyList<string> Labels { get; init; }

    [Description("更新者")]
    public UserId? UpdatedBy { get; init; }

    [Description("更新時間")]
    public DateTime? UpdatedOn { get; init; }
}

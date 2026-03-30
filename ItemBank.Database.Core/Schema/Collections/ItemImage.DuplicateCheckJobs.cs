using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemImage.DuplicateCheckJobs")]
[Description("題目圖片重複檢查工作")]
public sealed class ItemImageDuplicateCheckJob : IIndexable<ItemImageDuplicateCheckJob>
{
[BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedAt { get; init; }

    [Description("重複檢查 ErrorMessage")]
    public string? DuplicateCheckErrorMessage { get; init; }

    [Description("重複檢查狀態")]
    public required ItemImageJobStatus DuplicateCheckStatus { get; init; }

    [Description("圖片 Id 清單")]
    public required IReadOnlyList<BsonValue> ImageIds { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedAt { get; init; }
}

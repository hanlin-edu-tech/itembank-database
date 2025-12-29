using System.ComponentModel;
using System.Linq.Expressions;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("DocumentItemImportStatuses")]
[Description("五欄檔案題目匯入狀態檢視")]
public sealed class DocumentItemImportStatuses
{
    [BsonId]
    [Description("複合鍵")]
    public required CompositeKey Id { get; init; }

    [Description("匯入任務 Id")]
    public required TaskId TaskId { get; init; }

    [Description("匯入狀態")]
    public required ImportItemStatus Status { get; init; }

    [Description("失敗原因")]
    public required string Reason { get; init; }

    [Description("檔案名稱")]
    public required string FileName { get; init; }

    [Description("檔案 URL")]
    public required string FileUrl { get; init; }

    [Description("上傳者 Id")]
    public required UserId UploaderId { get; init; }

    [Description("上傳者名稱")]
    public required string UploaderName { get; init; }

    [Description("上傳時間")]
    public required DateTime UploadedAt { get; init; }

    [Description("重試次數")]
    public required int RetryCount { get; init; }

    [Description("完成時間")]
    public DateTime? FinishedAt { get; init; }

    [Description("五欄檔案題目複合鍵")]
    public sealed class CompositeKey
    {
        [Description("五欄檔案 Id")]
        public required DocumentId DocumentId { get; init; }

        [Description("題目 Id")]
        public required ItemId ItemId { get; init; }
    }
}
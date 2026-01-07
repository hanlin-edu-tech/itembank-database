using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Extensions;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ImportTasks")]
[Description("匯入任務")]
public class ImportTask : IIndexable<ImportTask>
{
    [BsonId]
    [Description("Id")]
    public required TaskId TaskId { get; init; }

    [Description("目錄群組代碼")]
    public required string CatalogGroupCode { get; init; }

    [Description("科目 Id 清單")]
    public required IReadOnlyList<SubjectId> SubjectIds { get; init; }

    [Description("五欄檔案 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("倉庫 Id")]
    public required RepositoryId RepositoryId { get; init; }

    [Description("匯入完成題目數")]
    public required int ImportFinishedItemCount { get; init; }

    [Description("總題目數")]
    public required int TotalItemCount { get; init; }

    [Description("匯入狀態")]
    public required ImportTaskStatus Status { get; init; }

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

    [Description("匯入完成時間")]
    public required DateTime? FinishedAt { get; init; }

    [Description("匯入日誌")]
    public required TaskLog[] Logs { get; init; }

    [Description("匯入題目")]
    public required Item[] Items { get; init; }

    static IReadOnlyList<CreateIndexModel<ImportTask>> IIndexable<ImportTask>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.Items.Select(item => item.Id))
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.SubjectIds)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.CatalogGroupCode)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.UploadedAt)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.UploaderId)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.UploaderName)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.FileName)
        ),
        new(
            Builders<ImportTask>.IndexKeys.Ascending(x => x.DocumentId)
        )
    ];

    public class TaskLog
    {
        [Description("時間")]
        public required DateTime Time { get; init; }

        [Description("訊息")]
        public required string Message { get; init; }
    }

    public class Item
    {
        [Description("題目 Id")]
        public required ItemId Id { get; init; }

        [Description("題目狀態")]
        public required ImportItemStatus Status { get; init; }

        [Description("更新時間")]
        public required DateTime UpdatedAt { get; init; }
    }
}
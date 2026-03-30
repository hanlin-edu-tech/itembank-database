using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ItemBankEvents")]
[CollectionSchemaKind(CollectionSchemaKind.DiscriminatedCollection)]
[CollectionSchemaDiscriminator("_t")]
[CollectionSchemaNote("依 _t 分為多個事件子型別")]
[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(ItemEvent), typeof(DocumentItemEvent), typeof(DeleteDocumentItemEvent))]
[Description("題庫事件基底")]
public abstract class ItemBankEvent : IIndexable<ItemBankEvent>
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("分類")]
    public required string Category { get; init; }

    [Description("題目 Id")]
    public required ItemId ItemId { get; init; }

    [Description("待處理任務清單")]
    public required IReadOnlyList<string> PendingTasks { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }

    static IReadOnlyList<CreateIndexModel<ItemBankEvent>> IIndexable<ItemBankEvent>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<ItemBankEvent>.IndexKeys.Ascending("category"),
                new CreateIndexOptions<ItemBankEvent>
                {
                    Name = "category_1"
                }
        ),
        new(
            Builders<ItemBankEvent>.IndexKeys.Ascending("itemId"),
                new CreateIndexOptions<ItemBankEvent>
                {
                    Name = "itemId_1"
                }
        ),
        new(
            Builders<ItemBankEvent>.IndexKeys.Ascending("pendingTasks"),
                new CreateIndexOptions<ItemBankEvent>
                {
                    Name = "pendingTasks_1"
                }
        ),
        new(
            Builders<ItemBankEvent>.IndexKeys.Ascending("updatedOn"),
                new CreateIndexOptions<ItemBankEvent>
                {
                    Name = "updatedOn_1"
                }
        )
    ];
}

[BsonDiscriminator("ItemEvent")]
[Description("題目事件")]
public sealed class ItemEvent : ItemBankEvent;

[BsonDiscriminator("DocumentItemEvent")]
[Description("文件題目事件")]
public sealed class DocumentItemEvent : ItemBankEvent
{
    [Description("文件 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("文件儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }
}

[BsonDiscriminator("DeleteDocumentItemEvent")]
[Description("刪除文件題目事件")]
public sealed class DeleteDocumentItemEvent : ItemBankEvent
{
    [Description("文件 Id")]
    public required DocumentId DocumentId { get; init; }

    [Description("文件儲存庫 Id")]
    public required DocumentRepoId DocumentRepoId { get; init; }
}

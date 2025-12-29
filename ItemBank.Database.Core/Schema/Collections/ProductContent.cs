using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("ProductContents")]
[Description("產品內容")]
public class ProductContent : IFinalizable, IAuditable
{
    [BsonId]
    [Description("Id")]
    public required ProductContentId Id { get; init; }

    [Description("是否已鎖定")]
    public bool IsFinalized { get; init; }

    [Description("鎖定時間")]
    public DateTime? FinalizedOn { get; init; }

    [Description("版本")]
    public int Revision { get; init; }

    [Description("產品 Id")]
    public required ProductId ProductId { get; init; }

    [Description("科目 Id")]
    public required SubjectId SubjectId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("期別")]
    public required TermEnum Term { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("版本 Id")]
    public required VersionId VersionId { get; init; }

    [Description("部門")]
    public required string Department { get; init; }

    [Description("編輯者")]
    public required UserId Editor { get; init; }

    [Description("預期鎖定日期")]
    public required DateTime ExpectedLockDate { get; init; }

    [Description("學期")]
    public required SemesterEnum Semester { get; init; }

    [Description("先修課程")]
    public required string? Prerequisite { get; init; }

    [Description("學程 Id")]
    public required BodyOfKnowledgeId? BodyOfKnowledgeId { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

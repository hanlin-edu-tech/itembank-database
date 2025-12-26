using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("產品內容")]
public class ProductContent : IFinalizable, IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("是否已鎖定")]
    public bool IsFinalized { get; set; }

    [Description("鎖定時間")]
    public DateTime? FinalizedOn { get; set; }

    [Description("版本")]
    public int Revision { get; set; }

    [Description("產品 Id")]
    public required string ProductId { get; init; }

    [Description("科目 Id")]
    public required string SubjectId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("期別")]
    public required TermEnum Term { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("版本 Id")]
    public required string VersionId { get; init; }

    [Description("部門")]
    public required string Department { get; init; }

    [Description("編輯者")]
    public required string Editor { get; init; }

    [Description("預期鎖定日期")]
    public required DateTime ExpectedLockDate { get; init; }

    [Description("學期")]
    public required SemesterEnum Semester { get; init; }

    [Description("先修課程")]
    public required string? Prerequisite { get; init; }

    [Description("學程 Id")]
    public required string? BodyOfKnowledgeId { get; init; }

    [Description("是否啟用")]
    public required bool Enabled { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

[Description("期別列舉")]
public enum TermEnum
{
    上期,
    下期
}

[Description("學期列舉")]
public enum SemesterEnum
{
    上學期,
    下學期,
    全部適用
}

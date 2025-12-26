using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("使用者類型")]
public class UserType : IFinalizable, IAuditable
{
    [BsonId]
    [Description("Id")]
    public required string Id { get; init; }

    [Description("科目 Id")]
    public required string SubjectId { get; init; }

    [Description("學年")]
    public required int Year { get; init; }

    [Description("是否已鎖定")]
    public bool IsFinalized { get; init; }

    [Description("鎖定時間")]
    public DateTime? FinalizedOn { get; init; }

    [Description("版本")]
    public int Revision { get; init; }

    [Description("建立者")]
    public required string CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required string UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

using System.ComponentModel;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("題型")]
public sealed class UserTypeValue : IAuditable
{
    [BsonId]
    [Description("Id")]
    public required UserTypeValueId Id { get; init; }

    [Description("題型表 Id")]
    public required UserTypeId UserTypeId { get; init; }

    [Description("名稱")]
    public required string Name { get; init; }

    [Description("分組名稱")]
    public required string GroupingName { get; init; }

    [Description("作答方式")]
    public required string? AnsweringMethod { get; init; }

    [Description("換行次數")]
    public required int NewlineCount { get; init; }

    [Description("是否應該加上括弧")]
    public required bool ShouldAddBracket { get; init; }

    [Description("描述")]
    public required string? Description { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("建立者")]
    public required UserId CreatedBy { get; init; }

    [Description("建立時間")]
    public required DateTime CreatedOn { get; init; }

    [Description("更新者")]
    public required UserId UpdatedBy { get; init; }

    [Description("更新時間")]
    public required DateTime UpdatedOn { get; init; }
}

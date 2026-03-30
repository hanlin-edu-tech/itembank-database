using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("AI 生成題目狀態")]
public enum GeneratedItemStatus
{
    [Description("草稿")]
    Draft,

    [Description("已發布")]
    Published
}

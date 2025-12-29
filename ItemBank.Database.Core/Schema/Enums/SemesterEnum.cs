using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("學期")]
public enum SemesterEnum
{
    [Description("上學期")]
    上學期,

    [Description("下學期")]
    下學期,

    [Description("全部適用")]
    全部適用
}

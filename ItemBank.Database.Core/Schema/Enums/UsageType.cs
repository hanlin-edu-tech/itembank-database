using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("使用類型")]
public enum UsageType
{
    [Description("常規")]
    Regular,

    [Description("必出")]
    Discrete
}

using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("版權類型")]
public enum CopyrightType
{
    [Description("無版權")]
    無版權 = 0,

    [Description("有版權")]
    有版權 = 1,

    [Description("翰教科")]
    翰教科 = 2,

    [Description("待談")]
    待談 = 3
}

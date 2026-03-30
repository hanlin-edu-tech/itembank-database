using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("上線準備度狀態")]
public enum OnlineReadinessStatus
{
    [Description("可上線")]
    Ready,

    [Description("不相容")]
    Incompatible,

    [Description("待審查")]
    PendingReview,

    [Description("需補強")]
    NeedWork,

    [Description("線上格式無效")]
    InvalidOnlineFormat
}

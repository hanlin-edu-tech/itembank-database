namespace ItemBank.Database.Core.Schema.Interfaces;

/// <summary>
/// 代表可鎖定的實體
/// </summary>
public interface IFinalizable
{
    /// <summary>取得表示此實體是否已鎖定的值</summary>
    bool IsFinalized { get; }

    /// <summary>取得此實體被鎖定的日期</summary>
    DateTime? FinalizedOn { get; }

    /// <summary>取得此實體的版本號</summary>
    int Revision { get; }
}

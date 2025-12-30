namespace ItemBank.Database.Core.Schema.ValueObjects.Abstractions;

/// <summary>
/// 可轉換的 Id 介面，定義 Id 與其底層值類型之間的轉換
/// </summary>
/// <typeparam name="TSelf">Id 型別本身</typeparam>
/// <typeparam name="TValue">底層值類型（如 string、ObjectId）</typeparam>
public interface IConvertibleId<TSelf, TValue>
    where TSelf : IConvertibleId<TSelf, TValue>
{
    /// <summary>
    /// 轉換為底層值類型
    /// </summary>
    TValue ToValue();

    /// <summary>
    /// 從底層值類型建立 Id 實例
    /// </summary>
    static abstract TSelf FromValue(TValue value);
}

namespace ItemBank.Database.Core.Schema.Enums;

public enum ImportTaskStatus
{
    匯入中,
    等待中,
    匯入失敗, // 最終狀態
    部分匯入失敗, // 最終狀態
    匯入成功, // 最終狀態
    正在進行重轉作業,
    已取消 // 最終狀態
}
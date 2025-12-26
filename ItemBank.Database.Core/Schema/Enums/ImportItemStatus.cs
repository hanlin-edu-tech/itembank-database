namespace ItemBank.Database.Core.Schema.Enums;

public enum ImportItemStatus
{
    等待中,
    待分題,
    分題失敗, // 最終狀態,
    待檢核,
    檢核中,
    紙本內容檢核失敗, // 最終狀態
    線上測驗檢核失敗, // 最終狀態
    發生未知錯誤, // 最終狀態
    匯入成功, // 最終狀態
    已取消, // 最終狀態
}
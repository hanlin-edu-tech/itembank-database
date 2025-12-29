using System.ComponentModel;

namespace ItemBank.Database.Core.Schema.Enums;

[Description("匯出類型")]
public enum ExportType
{
    [Description("題目")]
    Item,

    [Description("五欄檔案")]
    Document,

    [Description("五欄檔案儲存庫")]
    DocumentRepository,

    [Description("五欄檔案儲存庫包")]
    DocumentRepositoryPackage,

    [Description("五欄檔案問題題目")]
    DocumentIssueItems,

    [Description("五欄檔案匯入失敗題目")]
    DocumentFailImportItems,

    [Description("自訂檔案")]
    CustomFiles
}

using ItemBank.Database.Tools.SchemaDocGenerator.Models;

namespace ItemBank.Database.Tools.SchemaDocGenerator;

/// <summary>
/// Schema 分析器 - 掃描並分析所有集合類別
/// </summary>
public sealed class SchemaAnalyzer
{
    /// <summary>
    /// 分析所有集合定義
    /// </summary>
    /// <returns>Schema 文件（包含全局 Enum 和集合清單）</returns>
    public SchemaDocument AnalyzeCollections()
    {
        return SchemaRegistry.Build();
    }
}

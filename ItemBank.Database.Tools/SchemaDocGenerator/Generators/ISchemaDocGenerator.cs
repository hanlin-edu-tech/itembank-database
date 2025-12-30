using ItemBank.Database.Tools.SchemaDocGenerator.Models;

namespace ItemBank.Database.Tools.SchemaDocGenerator.Generators;

/// <summary>
/// Schema 文件生成器介面
/// </summary>
public interface ISchemaDocGenerator
{
    /// <summary>
    /// 產生 Schema 文件
    /// </summary>
    /// <param name="document">Schema 文件（包含全局 Enum 和集合清單）</param>
    /// <returns>文件內容字串</returns>
    string Generate(SchemaDocument document);
}

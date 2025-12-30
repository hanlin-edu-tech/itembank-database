using ItemBank.Database.Tools.SchemaDocGenerator.Generators;

namespace ItemBank.Database.Tools.SchemaDocGenerator;

/// <summary>
/// Schema 文件生成命令
/// </summary>
public sealed class SchemaDocCommand
{
    /// <summary>
    /// 執行 Schema 文件生成
    /// </summary>
    /// <param name="format">輸出格式 (yaml/md)</param>
    /// <param name="outputFile">輸出檔案路徑，若為 null 則輸出到 Console</param>
    public void Execute(string format, string? outputFile = null)
    {
        // 分析所有集合
        Console.WriteLine($"正在掃描集合定義...");
        var analyzer = new SchemaAnalyzer();
        var document = analyzer.AnalyzeCollections();
        Console.WriteLine($"找到 {document.Collections.Count} 個集合, {document.Enums.Count} 個 Enum 定義");

        // 選擇對應的生成器
        ISchemaDocGenerator generator = format.ToLowerInvariant() switch
        {
            "yaml" => new YamlSchemaGenerator(),
            "md" or "markdown" => new MarkdownSchemaGenerator(),
            _ => throw new ArgumentException($"不支援的格式: {format}。支援的格式: yaml, md")
        };

        // 產生文件
        Console.WriteLine($"正在生成 {format.ToUpperInvariant()} 格式文件...");
        var output = generator.Generate(document);

        // 輸出
        if (string.IsNullOrWhiteSpace(outputFile))
        {
            // 輸出到 Console
            Console.WriteLine();
            Console.WriteLine("=== 輸出結果 ===");
            Console.WriteLine();
            Console.WriteLine(output);
        }
        else
        {
            // 輸出到檔案
            try
            {
                File.WriteAllText(outputFile, output);
                Console.WriteLine($"✓ 文件已成功寫入: {Path.GetFullPath(outputFile)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 寫入檔案失敗: {ex.Message}");
                throw;
            }
        }
    }
}

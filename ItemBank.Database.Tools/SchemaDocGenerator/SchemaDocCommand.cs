using ItemBank.Database.Tools.SchemaDocGenerator.Generators;
using Spectre.Console;

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
        AnsiConsole.Write(new Rule("[bold cyan]Schema 文件生成工具[/]").RuleStyle("cyan"));
        AnsiConsole.WriteLine();

        // 分析所有集合
        Models.SchemaDocument document = null!;
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("[yellow]正在掃描集合定義...[/]", ctx =>
            {
                var analyzer = new SchemaAnalyzer();
                document = analyzer.AnalyzeCollections();
            });

        AnsiConsole.MarkupLine($"[dim]找到[/] [cyan]{document.Collections.Count}[/] [dim]個集合,[/] [cyan]{document.Enums.Count}[/] [dim]個 Enum 定義[/]");
        AnsiConsole.WriteLine();

        // 選擇對應的生成器
        ISchemaDocGenerator generator = format.ToLowerInvariant() switch
        {
            "yaml" => new YamlSchemaGenerator(),
            "md" or "markdown" => new MarkdownSchemaGenerator(),
            _ => throw new ArgumentException($"不支援的格式: {format}。支援的格式: yaml, md")
        };

        // 產生文件
        string output = null!;
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start($"[yellow]正在生成 {format.ToUpperInvariant()} 格式文件...[/]", ctx =>
            {
                output = generator.Generate(document);
            });

        // 輸出
        if (string.IsNullOrWhiteSpace(outputFile))
        {
            // 輸出到 Console
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[bold]輸出結果 ({format.ToUpperInvariant()})[/]").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(output);
        }
        else
        {
            // 輸出到檔案
            try
            {
                File.WriteAllText(outputFile, output);
                var fullPath = Path.GetFullPath(outputFile);

                AnsiConsole.WriteLine();
                var panel = new Panel($"[green]✓[/] 文件已成功寫入\n\n[dim]路徑:[/] [cyan]{fullPath}[/]\n[dim]格式:[/] [cyan]{format.ToUpperInvariant()}[/]\n[dim]大小:[/] [cyan]{new FileInfo(fullPath).Length:N0}[/] bytes")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green)
                    .Header("[bold green]完成[/]");

                AnsiConsole.Write(panel);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[red]✗ 寫入檔案失敗:[/] {Markup.Escape(ex.Message)}");
                throw;
            }
        }
    }
}

using ItemBank.Database.Core.Configuration;
using MongoDB.Driver;
using Spectre.Console;

namespace ItemBank.Database.Tools.IndexCreator;

/// <summary>
/// 創建索引命令
/// </summary>
public static class CreateIndexCommand
{
    private record IndexCreationResult(
        string CollectionName,
        int IndexCount,
        bool Success,
        string? ErrorMessage = null);

    /// <summary>
    /// 執行索引創建
    /// </summary>
    /// <param name="connectionString">MongoDB 連線字串</param>
    /// <param name="databaseName">資料庫名稱</param>
    public static async Task ExecuteAsync(string connectionString, string databaseName)
    {
        AnsiConsole.Write(new Rule("[bold cyan]MongoDB 索引創建工具[/]").RuleStyle("cyan"));
        AnsiConsole.WriteLine();

        try
        {
            // 建立 MongoDB 連線
            IMongoDatabase database = null!;
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("[yellow]正在連接到 MongoDB...[/]", async _ =>
                {
                    AnsiConsole.MarkupLine($"[dim]連線字串:[/] {MaskConnectionString(connectionString)}");
                    AnsiConsole.MarkupLine($"[dim]資料庫:[/] [cyan]{databaseName}[/]");

                    var client = new MongoClient(connectionString);
                    database = client.GetDatabase(databaseName);

                    await Task.Delay(100); // 讓使用者看到 spinner
                });

            AnsiConsole.MarkupLine("[green]✓[/] 連線成功");
            AnsiConsole.WriteLine();

            // 取得 Core 產生的索引清單
            var indexableEntries = IndexCreationRegistry.All;
            AnsiConsole.MarkupLine($"[dim]找到[/] [cyan]{indexableEntries.Length}[/] [dim]個需要建立索引的集合[/]");
            AnsiConsole.WriteLine();

            // 為每個集合建立索引
            var results = new List<IndexCreationResult>();

            await AnsiConsole.Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn())
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask("[cyan]建立索引中[/]", maxValue: indexableEntries.Length);

                    foreach (var entry in indexableEntries)
                    {
                        try
                        {
                            var (collectionName, indexCount) = await CreateIndexesAsync(database, entry);
                            results.Add(new IndexCreationResult(collectionName, indexCount, true));
                        }
                        catch (Exception ex)
                        {
                            results.Add(new IndexCreationResult(entry.CollectionName, 0, false, ex.Message));
                        }

                        task.Increment(1);
                    }
                });

            // 顯示結果表格
            AnsiConsole.WriteLine();
            DisplayResultsTable(results);

            // 顯示摘要
            var successCount = results.Count(r => r.Success);
            var failedCount = results.Count(r => !r.Success);

            AnsiConsole.WriteLine();
            var summaryTable = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn("[bold]項目[/]").Centered())
                .AddColumn(new TableColumn("[bold]數量[/]").Centered());

            summaryTable.AddRow("[green]成功[/]", $"[green]{successCount}[/]");
            summaryTable.AddRow(failedCount > 0 ? "[red]失敗[/]" : "[dim]失敗[/]",
                failedCount > 0 ? $"[red]{failedCount}[/]" : $"[dim]{failedCount}[/]");

            AnsiConsole.Write(summaryTable);

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(failedCount > 0 ? "[yellow]⚠[/] 部分索引建立失敗，請檢查上方錯誤訊息" : "[green]✓ 所有索引建立完成[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[red]✗ 執行失敗:[/] {ex.Message}");
            AnsiConsole.WriteException(ex);
            throw;
        }
    }

    private static void DisplayResultsTable(List<IndexCreationResult> results)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[bold]集合名稱[/]").LeftAligned())
            .AddColumn(new TableColumn("[bold]索引數[/]").Centered())
            .AddColumn(new TableColumn("[bold]狀態[/]").Centered());

        foreach (var result in results)
        {
            if (result.Success)
            {
                table.AddRow(
                    $"[cyan]{result.CollectionName}[/]",
                    $"{result.IndexCount}",
                    "[green]✓ 成功[/]"
                );
            }
            else
            {
                table.AddRow(
                    $"[yellow]{result.CollectionName}[/]",
                    "-",
                    $"[red]✗ 失敗[/]"
                );

                // 在表格後顯示錯誤訊息
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    AnsiConsole.MarkupLine($"  [dim]錯誤:[/] [red]{Markup.Escape(result.ErrorMessage)}[/]");
                }
            }
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// 為指定集合建立索引
    /// </summary>
    /// <returns>集合名稱和索引數量</returns>
    private static async Task<(string CollectionName, int IndexCount)> CreateIndexesAsync(IMongoDatabase database,
        IndexCreationEntry entry)
    {
        var indexCount = await entry.CreateAsync(database, CancellationToken.None);
        return (entry.CollectionName, indexCount);
    }

    /// <summary>
    /// 遮罩連線字串中的敏感資訊
    /// </summary>
    private static string MaskConnectionString(string connectionString)
    {
        // 簡單遮罩密碼部分
        var parts = connectionString.Split('@');
        if (parts.Length <= 1) return connectionString;
        var credentialParts = parts[0].Split("://");
        return credentialParts.Length > 1 ? $"{credentialParts[0]}://***@{parts[1]}" : connectionString;
    }
}
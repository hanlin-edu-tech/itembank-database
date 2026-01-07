using ItemBank.Database.Core.Configuration;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Driver;
using Spectre.Console;
using System.Reflection;

namespace ItemBank.Database.Tools.IndexCreator;

/// <summary>
/// 創建索引命令
/// </summary>
public sealed class CreateIndexCommand
{
    private record IndexCreationResult(string CollectionName, int IndexCount, bool Success, string? ErrorMessage = null);

    /// <summary>
    /// 執行索引創建
    /// </summary>
    /// <param name="connectionString">MongoDB 連線字串</param>
    /// <param name="databaseName">資料庫名稱</param>
    public async Task ExecuteAsync(string connectionString, string databaseName)
    {
        AnsiConsole.Write(new Rule("[bold cyan]MongoDB 索引創建工具[/]").RuleStyle("cyan"));
        AnsiConsole.WriteLine();

        try
        {
            // 建立 MongoDB 連線
            IMongoDatabase database = null!;
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("[yellow]正在連接到 MongoDB...[/]", async ctx =>
                {
                    AnsiConsole.MarkupLine($"[dim]連線字串:[/] {MaskConnectionString(connectionString)}");
                    AnsiConsole.MarkupLine($"[dim]資料庫:[/] [cyan]{databaseName}[/]");

                    var client = new MongoClient(connectionString);
                    database = client.GetDatabase(databaseName);

                    await Task.Delay(100); // 讓使用者看到 spinner
                });

            AnsiConsole.MarkupLine("[green]✓[/] 連線成功");
            AnsiConsole.WriteLine();

            // 掃描所有 IIndexable 類型
            var indexableTypes = FindAllIndexableTypes().ToList();
            AnsiConsole.MarkupLine($"[dim]找到[/] [cyan]{indexableTypes.Count}[/] [dim]個需要建立索引的集合[/]");
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
                    var task = ctx.AddTask("[cyan]建立索引中[/]", maxValue: indexableTypes.Count);

                    foreach (var type in indexableTypes)
                    {
                        try
                        {
                            var (collectionName, indexCount) = await CreateIndexesAsync(database, type);
                            results.Add(new IndexCreationResult(collectionName, indexCount, true));
                        }
                        catch (Exception ex)
                        {
                            var collectionName = GetCollectionName(type);
                            results.Add(new IndexCreationResult(collectionName, 0, false, ex.Message));
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
            summaryTable.AddRow(failedCount > 0 ? "[red]失敗[/]" : "[dim]失敗[/]", failedCount > 0 ? $"[red]{failedCount}[/]" : $"[dim]{failedCount}[/]");

            AnsiConsole.Write(summaryTable);

            if (failedCount > 0)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[yellow]⚠[/] 部分索引建立失敗，請檢查上方錯誤訊息");
            }
            else
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[green]✓ 所有索引建立完成[/]");
            }
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
    /// 為指定類型建立索引
    /// </summary>
    /// <returns>集合名稱和索引數量</returns>
    private static async Task<(string CollectionName, int IndexCount)> CreateIndexesAsync(IMongoDatabase database, Type documentType)
    {
        // 獲取集合名稱
        var collectionName = GetCollectionName(documentType);

        // 獲取 IIndexable<T> 接口
        var indexableInterface = typeof(IIndexable<>).MakeGenericType(documentType);

        if (!documentType.GetInterfaces().Contains(indexableInterface))
        {
            throw new InvalidOperationException($"類型 {documentType.Name} 未實作 IIndexable");
        }

        // 獲取集合
        var getCollectionMethod = typeof(IMongoDatabase)
            .GetMethod(nameof(IMongoDatabase.GetCollection))!
            .MakeGenericMethod(documentType);

        var collection = getCollectionMethod.Invoke(database, [collectionName, null]);
        if (collection == null)
        {
            throw new InvalidOperationException("無法取得集合");
        }

        // 獲取靜態屬性 CreateIndexModels
        var createIndexModelsProperty = indexableInterface.GetProperty(
            "CreateIndexModels",
            BindingFlags.Public | BindingFlags.Static);

        if (createIndexModelsProperty == null)
        {
            throw new InvalidOperationException("無索引定義");
        }

        // 取得索引模型列表
        var indexModels = createIndexModelsProperty.GetValue(null);
        if (indexModels == null)
        {
            throw new InvalidOperationException("無索引");
        }

        // 呼叫 collection.Indexes.CreateManyAsync(indexModels)
        var indexesProperty = collection.GetType().GetProperty("Indexes");
        if (indexesProperty == null)
        {
            throw new InvalidOperationException("無法取得索引管理器");
        }

        var indexManager = indexesProperty.GetValue(collection);
        if (indexManager == null)
        {
            throw new InvalidOperationException("索引管理器為 null");
        }

        var createManyMethod = indexManager.GetType().GetMethod(
            "CreateManyAsync",
            [indexModels.GetType(), typeof(CancellationToken)]);

        if (createManyMethod == null)
        {
            throw new InvalidOperationException("無法找到 CreateManyAsync 方法");
        }

        await (Task)createManyMethod.Invoke(indexManager, [indexModels, CancellationToken.None])!;

        // 取得索引數量
        var indexModelsList = indexModels as System.Collections.IEnumerable;
        var count = indexModelsList?.Cast<object>().Count() ?? 0;

        return (collectionName, count);
    }

    /// <summary>
    /// 獲取集合名稱
    /// </summary>
    private static string GetCollectionName(Type documentType)
    {
        var collectionNameAttr = documentType.GetCustomAttributes(typeof(ItemBank.Database.Core.Schema.Attributes.CollectionNameAttribute), false)
            .FirstOrDefault() as ItemBank.Database.Core.Schema.Attributes.CollectionNameAttribute;

        return collectionNameAttr?.Name ?? documentType.Name;
    }

    /// <summary>
    /// 掃描並尋找所有實作 IIndexable&lt;T&gt; 的類型
    /// </summary>
    private static IEnumerable<Type> FindAllIndexableTypes()
    {
        var assembly = typeof(DbContext).Assembly;
        var indexableGenericInterface = typeof(IIndexable<>);

        return assembly.GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == indexableGenericInterface));
    }

    /// <summary>
    /// 遮罩連線字串中的敏感資訊
    /// </summary>
    private static string MaskConnectionString(string connectionString)
    {
        // 簡單遮罩密碼部分
        var parts = connectionString.Split('@');
        if (parts.Length > 1)
        {
            var credentialParts = parts[0].Split("://");
            if (credentialParts.Length > 1)
            {
                return $"{credentialParts[0]}://***@{parts[1]}";
            }
        }

        return connectionString;
    }
}

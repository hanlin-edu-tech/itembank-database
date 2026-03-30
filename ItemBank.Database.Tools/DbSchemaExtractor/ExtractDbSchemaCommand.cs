using System.Text.Json;
using System.Text.Json.Serialization;
using ItemBank.Database.Tools.DbSchemaExtractor.Models;
using Spectre.Console;

namespace ItemBank.Database.Tools.DbSchemaExtractor;

public static class ExtractDbSchemaCommand
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static async Task ExecuteAsync(
        string connectionString,
        string databaseName,
        int sampleSize,
        string? outputFile,
        IReadOnlyCollection<string>? collectionNames)
    {
        AnsiConsole.Write(new Rule("[bold cyan]MongoDB Schema 擷取工具[/]").RuleStyle("cyan"));
        AnsiConsole.WriteLine();

        try
        {
            AnsiConsole.MarkupLine($"[dim]連線字串:[/] {MaskConnectionString(connectionString)}");
            AnsiConsole.MarkupLine($"[dim]資料庫:[/] [cyan]{databaseName}[/]");
            AnsiConsole.MarkupLine($"[dim]每個集合抽樣筆數:[/] [cyan]{sampleSize}[/]");

            if (collectionNames is { Count: > 0 })
            {
                var collectionText = string.Join(", ", collectionNames.Order(StringComparer.Ordinal));
                AnsiConsole.MarkupLine($"[dim]指定集合:[/] [cyan]{Markup.Escape(collectionText)}[/]");
            }

            AnsiConsole.WriteLine();

            ExtractedDatabaseSchema schema = null!;
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("[yellow]正在擷取資料庫 schema...[/]", async _ =>
                {
                    var extractor = new DbSchemaExtractor();
                    schema = await extractor
                        .ExtractAsync(connectionString, databaseName, sampleSize, collectionNames)
                        .ConfigureAwait(false);
                })
                .ConfigureAwait(false);

            var extractedCollectionCount = schema.Collections.Count;
            AnsiConsole.MarkupLine($"[green]✓[/] 已擷取 [cyan]{extractedCollectionCount}[/] 個集合");

            if (collectionNames is { Count: > 0 })
            {
                var extractedCollectionNames = schema.Collections
                    .Select(collection => collection.Name)
                    .ToHashSet(StringComparer.Ordinal);
                var missingCollections = collectionNames
                    .Where(name => !extractedCollectionNames.Contains(name))
                    .Order(StringComparer.Ordinal)
                    .ToArray();

                if (missingCollections.Length > 0)
                {
                    AnsiConsole.MarkupLine(
                        $"[yellow]⚠[/] 找不到指定集合: [yellow]{Markup.Escape(string.Join(", ", missingCollections))}[/]");
                }
            }

            var json = JsonSerializer.Serialize(schema, JsonOptions);

            if (string.IsNullOrWhiteSpace(outputFile))
            {
                AnsiConsole.WriteLine();
                Console.WriteLine(json);
                return;
            }

            var fullPath = Path.GetFullPath(outputFile);
            var outputDirectory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            await File.WriteAllTextAsync(fullPath, json).ConfigureAwait(false);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Panel(
                    $"[green]✓[/] JSON 事實檔已成功寫入\n\n[dim]路徑:[/] [cyan]{Markup.Escape(fullPath)}[/]\n[dim]集合數:[/] [cyan]{schema.Collections.Count}[/]\n[dim]大小:[/] [cyan]{new FileInfo(fullPath).Length:N0}[/] bytes")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Green)
                .Header("[bold green]完成[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[red]✗ 執行失敗:[/] {Markup.Escape(ex.Message)}");
            throw;
        }
    }

    private static string MaskConnectionString(string connectionString)
    {
        var parts = connectionString.Split('@');
        if (parts.Length <= 1)
            return connectionString;

        var credentialParts = parts[0].Split("://");
        return credentialParts.Length > 1 ? $"{credentialParts[0]}://***@{parts[1]}" : connectionString;
    }
}

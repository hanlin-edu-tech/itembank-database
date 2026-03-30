using ItemBank.Database.Core.Configuration;
using ItemBank.Database.Tools.DbSchemaExtractor;
using ItemBank.Database.Core.Schema.Collections;
using ItemBank.Database.Core.Schema.Interfaces;
using ItemBank.Database.Tools.IndexCreator;
using ItemBank.Database.Tools.SchemaDocGenerator;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

// ItemBank Database Tools - Schema & Migration Analysis
// CLI 入口

// 設置依賴注入容器
var services = new ServiceCollection();

// 註冊 MongoDB 序列化器
MongoDbExtensions.RegisterSerializers();

// 建立 ServiceProvider
var serviceProvider = services.BuildServiceProvider();

// 如果沒有參數，顯示使用說明
if (args.Length < 1)
{
    Console.WriteLine("ItemBank Database Tools");
    Console.WriteLine();
    Console.WriteLine("可用命令:");
    Console.WriteLine();
    Console.WriteLine("  schema-doc    生成 Schema 文件");
    Console.WriteLine("  create-index  創建資料庫索引");
    Console.WriteLine("  extract-db-schema  從資料庫擷取 Schema 事實檔");
    Console.WriteLine();
    Console.WriteLine("用法:");
    Console.WriteLine();
    Console.WriteLine("  schema-doc [-f|--format yaml|md|json] [-o|--output <filename>]");
    Console.WriteLine("    -f, --format <format>    輸出格式 (yaml|md|json)，預設：yaml");
    Console.WriteLine("    -o, --output <filename>  輸出到檔案，若未指定則輸出到 Console");
    Console.WriteLine();
    Console.WriteLine("  create-index -c|--connection <connection-string> -d|--database <database-name>");
    Console.WriteLine("    -c, --connection <string>  MongoDB 連線字串");
    Console.WriteLine("    -d, --database <name>      資料庫名稱");
    Console.WriteLine();
    Console.WriteLine("  extract-db-schema -c|--connection <connection-string> -d|--database <database-name> [-o|--output <filename>] [--sample-size <count>] [--collections <name1,name2,...>]");
    Console.WriteLine("    -c, --connection <string>      MongoDB 連線字串");
    Console.WriteLine("    -d, --database <name>          資料庫名稱");
    Console.WriteLine("    -o, --output <filename>        輸出 JSON 事實檔，若未指定則輸出到 Console");
    Console.WriteLine("        --sample-size <count>      每個集合抽樣文件數，預設：100");
    Console.WriteLine("        --collections <names>      僅擷取指定集合，使用逗號分隔");
    Console.WriteLine();
    Console.WriteLine("範例:");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc -f yaml -o schema.yaml");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- create-index -c \"mongodb://localhost:27017\" -d itembank");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- extract-db-schema -c \"mongodb://localhost:27017\" -d itembank -o schema-facts.json --sample-size 100");
    return;
}

// 處理命令
var command = args[0].ToLowerInvariant();

switch (command)
{
    case "schema-doc":
        ExecuteSchemaDoc(args);
        break;

    case "create-index":
        await ExecuteCreateIndexAsync(args);
        break;

    case "extract-db-schema":
        await ExecuteExtractDbSchemaAsync(args);
        break;

    default:
        Console.WriteLine($"未知的命令: {command}");
        Console.WriteLine("執行 'dotnet run' 查看可用命令");
        break;
}

return;

static void ExecuteSchemaDoc(string[] cmdArgs)
{
    var format = "yaml"; // 預設格式
    string? outputFile = null;

    // 解析參數
    for (var i = 1; i < cmdArgs.Length; i++)
    {
        switch (cmdArgs[i])
        {
            case "--format" or "-f" when i + 1 < cmdArgs.Length:
                format = cmdArgs[i + 1];
                i++; // 跳過下一個參數（已被使用）
                break;
            case "--output" or "-o" when i + 1 < cmdArgs.Length:
                outputFile = cmdArgs[i + 1];
                i++; // 跳過下一個參數（已被使用）
                break;
        }
    }

    var command = new SchemaDocCommand();
    command.Execute(format, outputFile);
}

static async Task ExecuteCreateIndexAsync(string[] cmdArgs)
{
    string? connectionString = null;
    string? databaseName = null;

    // 解析參數
    for (var i = 1; i < cmdArgs.Length; i++)
    {
        switch (cmdArgs[i])
        {
            case "--connection" or "-c" when i + 1 < cmdArgs.Length:
                connectionString = cmdArgs[i + 1];
                i++; // 跳過下一個參數（已被使用）
                break;
            case "--database" or "-d" when i + 1 < cmdArgs.Length:
                databaseName = cmdArgs[i + 1];
                i++; // 跳過下一個參數（已被使用）
                break;
        }
    }

    // 驗證必要參數
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("錯誤: 缺少必要參數 --connection (-c)");
        Console.WriteLine("執行 'dotnet run' 查看使用說明");
        return;
    }

    if (string.IsNullOrWhiteSpace(databaseName))
    {
        Console.WriteLine("錯誤: 缺少必要參數 --database (-d)");
        Console.WriteLine("執行 'dotnet run' 查看使用說明");
        return;
    }

    await CreateIndexCommand.ExecuteAsync(connectionString, databaseName);
}

static async Task ExecuteExtractDbSchemaAsync(string[] cmdArgs)
{
    string? connectionString = null;
    string? databaseName = null;
    string? outputFile = null;
    string? collectionsArgument = null;
    var sampleSize = 100;

    for (var i = 1; i < cmdArgs.Length; i++)
    {
        switch (cmdArgs[i])
        {
            case "--connection" or "-c" when i + 1 < cmdArgs.Length:
                connectionString = cmdArgs[i + 1];
                i++;
                break;
            case "--database" or "-d" when i + 1 < cmdArgs.Length:
                databaseName = cmdArgs[i + 1];
                i++;
                break;
            case "--output" or "-o" when i + 1 < cmdArgs.Length:
                outputFile = cmdArgs[i + 1];
                i++;
                break;
            case "--sample-size" when i + 1 < cmdArgs.Length:
                if (!int.TryParse(cmdArgs[i + 1], out sampleSize) || sampleSize <= 0)
                {
                    Console.WriteLine("錯誤: --sample-size 必須是大於 0 的整數");
                    return;
                }

                i++;
                break;
            case "--collections" when i + 1 < cmdArgs.Length:
                collectionsArgument = cmdArgs[i + 1];
                i++;
                break;
        }
    }

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("錯誤: 缺少必要參數 --connection (-c)");
        Console.WriteLine("執行 'dotnet run' 查看使用說明");
        return;
    }

    if (string.IsNullOrWhiteSpace(databaseName))
    {
        Console.WriteLine("錯誤: 缺少必要參數 --database (-d)");
        Console.WriteLine("執行 'dotnet run' 查看使用說明");
        return;
    }

    var collectionNames = string.IsNullOrWhiteSpace(collectionsArgument)
        ? null
        : collectionsArgument
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    await ExtractDbSchemaCommand.ExecuteAsync(connectionString, databaseName, sampleSize, outputFile, collectionNames);
}

using ItemBank.Database.Core.Configuration;
using ItemBank.Database.Tools.SchemaDocGenerator;
using Microsoft.Extensions.DependencyInjection;

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
    Console.WriteLine("用法:");
    Console.WriteLine("  schema-doc [-f|--format yaml|md] [-o|--output <filename>]");
    Console.WriteLine();
    Console.WriteLine("參數:");
    Console.WriteLine("  -f, --format <format>    輸出格式 (yaml|md)，預設：yaml");
    Console.WriteLine("  -o, --output <filename>  輸出到檔案，若未指定則輸出到 Console");
    Console.WriteLine();
    Console.WriteLine("範例:");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc -f yaml");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc --format md");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc -f yaml -o schema.yaml");
    Console.WriteLine("  dotnet run --project ItemBank.Database.Tools -- schema-doc --format md --output schema.md");
    return;
}

// 處理命令
var command = args[0].ToLowerInvariant();

switch (command)
{
    case "schema-doc":
        ExecuteSchemaDoc(args);
        break;

    default:
        Console.WriteLine($"未知的命令: {command}");
        Console.WriteLine("執行 'dotnet run' 查看可用命令");
        break;
}

/// <summary>
/// 執行 Schema 文件生成命令
/// </summary>
static void ExecuteSchemaDoc(string[] cmdArgs)
{
    var format = "yaml"; // 預設格式
    string? outputFile = null;

    // 解析參數
    for (int i = 1; i < cmdArgs.Length; i++)
    {
        if ((cmdArgs[i] == "--format" || cmdArgs[i] == "-f") && i + 1 < cmdArgs.Length)
        {
            format = cmdArgs[i + 1];
            i++; // 跳過下一個參數（已被使用）
        }
        else if ((cmdArgs[i] == "--output" || cmdArgs[i] == "-o") && i + 1 < cmdArgs.Length)
        {
            outputFile = cmdArgs[i + 1];
            i++; // 跳過下一個參數（已被使用）
        }
    }

    var command = new SchemaDocCommand();
    command.Execute(format, outputFile);
}

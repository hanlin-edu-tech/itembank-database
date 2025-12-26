# ItemBank.Database.Core

MongoDB 資料庫核心類庫，提供統一的集合管理、索引定義和依賴注入整合。

## 功能

### 🗂️ Schema 管理
- **Collections** - 定義 MongoDB 集合的資料模型
- **Models** - 共用領域模型
- **CollectionNameAttribute** - 指定集合的自訂名稱

### 📇 索引管理
- **IIndexable\<T\>** - 靜態抽象方法，定義集合索引
- 自動掃描和初始化所有索引
- 後台服務執行，不阻塞啟動

### ⚙️ 配置整合
- **DbContext** - MongoDB 資料庫上下文
- **MongoDbContextOptions** - 配置選項
- **AddMongoDbContext** - 依賴注入擴展方法

## 快速開始

### 1. 定義集合模型

```csharp
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Driver;

namespace MyApp.Database;

[CollectionName("users")]
public class User : IIndexable<User>
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public static async Task CreateIndexesAsync(IMongoCollection<User> collection)
    {
        // 定義索引
        var indexModel = new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Ascending(u => u.Email),
            new CreateIndexOptions { Unique = true }
        );

        await collection.Indexes.CreateOneAsync(indexModel);
    }
}
```

### 2. 配置依賴注入

```csharp
var services = new ServiceCollection();

services.AddMongoDbContext(options =>
{
    options
        .WithClientSettings(MongoClientSettings.FromConnectionString("mongodb://localhost:27017"))
        .WithDatabaseName("itembank")
        .WithAutoCreateIndexes(true); // 啟用自動建立索引
});

var provider = services.BuildServiceProvider();
```

### 3. 使用 DbContext

```csharp
var dbContext = provider.GetRequiredService<DbContext>();

// 取得集合
var usersCollection = dbContext.GetCollection<User>();

// 插入文件
await usersCollection.InsertOneAsync(new User
{
    Name = "John Doe",
    Email = "john@example.com"
});

// 查詢
var user = await usersCollection.Find(u => u.Email == "john@example.com").FirstOrDefaultAsync();
```

## 架構說明

### 目錄結構

```
ItemBank.Database.Core/
├── Schema/
│   ├── Attributes/
│   │   └── CollectionNameAttribute.cs      # 集合名稱屬性
│   ├── Collections/                        # 集合模型存放處
│   ├── Models/                             # 共用資料模型存放處
│   └── Interfaces/
│       └── IIndexable.cs                   # 索引定義介面
├── Indexes/                                # 索引工具（未來擴展）
├── Configuration/
│   ├── DbContext.cs                        # 資料庫上下文
│   ├── MongoDbContextOptions.cs            # 配置選項
│   ├── MongoDbExtensions.cs                # DI 擴展方法
│   ├── IndexInitializationService.cs       # 後台索引初始化服務
│   └── BsonSerializers/                    # BSON 序列化器存放處
└── ItemBank.Database.Core.csproj
```

### 索引初始化流程

1. **應用啟動**
   - DI 容器載入 `MongoDbContextOptions`
   - 如果 `AutoCreateIndexes = true`，註冊 `IndexInitializationService`

2. **後台服務執行**
   - `IndexInitializationService` 作為 `IHostedService` 在應用啟動時執行
   - 掃描 Core 程式集中所有實作 `IIndexable<T>` 的類型

3. **索引建立**
   - 對每個類型，呼叫其靜態方法 `CreateIndexesAsync`
   - 非同步執行，不阻塞主線程

## 依賴套件

- **MongoDB.Driver** (3.5.2+) - MongoDB .NET 驅動程式
- **Microsoft.Extensions.DependencyInjection** (10.0+) - 依賴注入
- **Microsoft.Extensions.Hosting** (10.0+) - 主機服務

## 設計考量

### 為什麼使用 Static Abstract 方法？

- ✅ 不需要建立集合類的實例
- ✅ 編譯時檢查實作
- ✅ 更清晰的意圖（索引定義是類級別的責任）

### 為什麼使用後台服務初始化索引？

- ✅ 不阻塞 DI 容器初始化
- ✅ 非同步執行，更好的效能
- ✅ 可控制初始化行為（透過 `AutoCreateIndexes` 選項）

### CollectionName 屬性用途

- 允許自訂集合名稱（預設為類名）
- 與資料庫命名規範解耦
- 示例：`[CollectionName("users")]` vs 類名 `User`

## 擴展性

未來可擴展的方向：

- `BsonSerializers/` - 自訂 BSON 序列化器
- `Indexes/` - 統一索引定義工具類
- `Migrations/` - 資料庫遷移支援
- `Transactions/` - 事務支援

## 注意事項

⚠️ **AutoCreateIndexes 預設為 false**
- 生產環境應顯式啟用
- 避免意外的索引建立

⚠️ **集合類應可無參數實例化**
- 用於掃描 `IIndexable<T>` 實作時
- 或使用 `Activator.CreateInstance` 兼容的設計

⚠️ **索引定義應為冪等性**
- MongoDB 會自動跳過已存在的索引
- 但應避免修改現有索引定義

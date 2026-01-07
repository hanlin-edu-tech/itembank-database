# ItemBank.Database.Tools

資料庫工具集，提供 Schema 文件生成和資料庫遷移分析功能。

## 功能概述

### 📄 SchemaDocGenerator - Schema 文件生成工具

自動掃描 `ItemBank.Database.Core` 中定義的所有集合，產生結構化的 Schema 文件。

**用途：**
- 自動化文件維護
- 開發團隊快速了解資料結構
- 生成 Markdown 格式的 Schema 說明

**輸出內容：**
- 集合名稱
- 欄位定義（名稱、型別、說明）
- 索引定義
- 關聯關係

**使用方式：**
```bash
dotnet run --project ItemBank.Database.Tools -- schema-doc
dotnet run --project ItemBank.Database.Tools -- schema-doc -f yaml -o schema.yaml
```

---

### 🔧 IndexCreator - 索引創建工具

掃描所有實作 `IIndexable<T>` 的集合定義，並在 MongoDB 資料庫中創建對應的索引。

**用途：**
- 初始化新資料庫的索引
- 補建遺失的索引
- 更新索引定義

**功能特點：**
- 自動掃描所有 IIndexable 集合
- 顯示詳細的執行進度
- 統計成功/失敗數量
- 支援自定義 MongoDB 連線

**使用方式：**
```bash
dotnet run --project ItemBank.Database.Tools -- create-index -c "mongodb://localhost:27017" -d itembank
```

**參數說明：**
- `-c, --connection <string>` - MongoDB 連線字串（必要）
- `-d, --database <name>` - 資料庫名稱（必要）

---

### 🔍 MigrationAnalyzer - 遷移資料分析工具

掃描新舊資料庫，比對資料一致性，協助遷移前的資料分析。

#### NewDbAnalyzer - 新總庫掃描

**用途：**
- 掃描新總庫中的資料
- 識別需要清理的資料（重複、無效、孤立）
- 產生資料品質報告

**分析項目：**
- 重複資料
- 欄位缺失
- 參考完整性
- 資料格式問題

**使用方式：**
```bash
dotnet run --project ItemBank.Database.Tools -- analyze-new --connection "mongodb://..."
```

#### LegacyDbAnalyzer - 舊總庫掃描

**用途：**
- 掃描舊總庫中的資料
- 識別遷移時需要處理的資料
- 產生需要人工介入的清單

**分析項目：**
- 缺失的必要欄位
- 需要轉換的資料格式
- 無法自動遷移的資料
- 需要人工補充的資料

**使用方式：**
```bash
dotnet run --project ItemBank.Database.Tools -- analyze-legacy --connection "mongodb://..."
```

---

## 目錄結構

```
ItemBank.Database.Tools/
├── SchemaDocGenerator/          # Schema 文件生成器
│   ├── Generators/
│   ├── Models/
│   ├── TypeMappers/
│   ├── SchemaAnalyzer.cs
│   └── SchemaDocCommand.cs
├── IndexCreator/                # 索引創建工具
│   └── CreateIndexCommand.cs
├── MigrationAnalyzer/           # 遷移資料分析
│   ├── (待實作)
│   ├── NewDbAnalyzer.cs        # 新總庫分析
│   └── LegacyDbAnalyzer.cs     # 舊總庫分析
├── Program.cs                   # CLI 入口
└── ItemBank.Database.Tools.csproj
```

## 依賴

- **ItemBank.Database.Core** - 引用 Schema 定義
- **MongoDB.Driver** - 資料庫連線
- **System.CommandLine** (未來) - CLI 參數解析

## 輸出格式

所有工具皆輸出至 **Console**，方便管道處理與日誌記錄。

**範例：**
```
[SchemaDocGenerator] 開始掃描 Schema...
[SchemaDocGenerator] 找到 15 個集合定義
[SchemaDocGenerator] 生成文件完成

[NewDbAnalyzer] 連接至新總庫...
[NewDbAnalyzer] 發現 3 筆重複資料
[NewDbAnalyzer] 發現 12 筆欄位缺失
```

## 開發狀態

✅ **SchemaDocGenerator** - 已完成
✅ **IndexCreator** - 已完成
🚧 **MigrationAnalyzer** - 規劃中

預計實作順序：
1. ✅ SchemaDocGenerator - 提供文件化支援
2. ✅ IndexCreator - 索引管理工具
3. 🚧 NewDbAnalyzer - 新庫資料品質檢查
4. 🚧 LegacyDbAnalyzer - 舊庫遷移準備分析

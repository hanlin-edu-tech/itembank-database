# BSON Serializer 指南

此文件說明在新增 Collection 或需要自訂序列化邏輯時，如何建立和註冊 BSON Serializer。

## 概述

BSON Serializer 用於自訂 MongoDB 驅動程式如何序列化和反序列化 .NET 物件。當需要特殊的資料轉換邏輯（如 Enum 轉字符串、ValueObject 轉原始值等）時，需要建立自訂 Serializer。

## 目錄結構

所有 BSON Serializer 應放在 `Configuration/BsonSerializers/` 目錄下：

```
ItemBank.Database.Core/
├── Configuration/
│   └── BsonSerializers/
│       ├── CamelCaseEnumStringSerializer.cs      # 通用 Enum Serializer
│       ├── SubjectIdSerializer.cs                # ValueObject Serializer 範例
│       ├── DimensionIdSerializer.cs              # ValueObject Serializer 範例
│       └── ... 其他 Serializer
```

## 可用的 Serializer 工具

### 1. CamelCaseEnumStringSerializer<TEnum>

將 Enum 序列化為 camelCase 字符串格式。

**用途**：當需要 Enum 字段轉換為 camelCase 字符串儲存於 MongoDB 時使用。

**特性**：
- 支持 nullable Enum
- 自動 Enum 名稱轉 camelCase（如 `Knowledge` → `"knowledge"`）
- 反序列化時支持大小寫不敏感的解析

**使用範例**：

```csharp
public enum DimensionType
{
    Knowledge,
    Lesson,
    Recognition
}

// 在 Collection 中使用
public class Dimension
{
    public required DimensionType Type { get; init; }
}

// 在 MongoDbExtensions.cs 中註冊
BsonSerializer.RegisterSerializer(new CamelCaseEnumStringSerializer<DimensionType>());
```

**資料庫儲存效果**：
```json
{ "type": "knowledge" }  // camelCase
```

### 2. NullableClassSerializerBase<T>

為 Class 型別 ValueObject 提供的基礎類別，用於自訂序列化邏輯。

**用途**：當 ValueObject 需要轉換為原始值（如 String、ObjectId）儲存時使用。

**使用範例**：

```csharp
// ValueObject 定義
public class UserId
{
    public string Value { get; }
    public UserId(string value) => Value = value;
}

// Serializer 實作
public class UserIdSerializer : NullableClassSerializerBase<UserId>
{
    protected override UserId DeserializeValue(
        BsonDeserializationContext context,
        BsonDeserializationArgs args,
        BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new UserId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 UserId");
    }

    protected override void SerializeValue(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        UserId value)
    {
        context.Writer.WriteString(value.Value);
    }
}

// 在 MongoDbExtensions.cs 中註冊
BsonSerializer.RegisterSerializer(new UserIdSerializer());
```

**資料庫儲存效果**：
```json
{ "createdBy": "user-123" }  // 直接序列化為字符串
```

## 建立自訂 Serializer

### 步驟 1：建立 Serializer 類

在 `Configuration/BsonSerializers/` 下建立新檔案。命名規則為 `{TypeName}Serializer.cs`。

```csharp
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

public class MyCustomSerializer : SerializerBase<MyType>
{
    public override MyType Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        // 反序列化邏輯
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, MyType value)
    {
        // 序列化邏輯
    }
}
```

### 步驟 2：在 MongoDbExtensions.cs 中註冊

在 `Configuration/MongoDbExtensions.cs` 的 `RegisterSerializers()` 方法中，於適當位置加入註冊：

```csharp
private static void RegisterSerializers()
{
    // ... 其他 Serializer 註冊 ...

    // 新增 Serializer 註冊
    BsonSerializer.RegisterSerializer(new MyCustomSerializer());

    // ... 後續程式碼 ...
}
```

## 實務案例：Enum Serializer

本案例展示如何為 `DimensionType` Enum 建立和註冊 Serializer。

### Collection 定義

```csharp
// Schema/Enums/DimensionType.cs
namespace ItemBank.Database.Core.Schema.Enums;

public enum DimensionType
{
    Knowledge,
    Lesson,
    Recognition
}

// Schema/Collections/Dimension.cs
using ItemBank.Database.Core.Schema.Enums;

public sealed class Dimension : IFinalizable, IAuditable
{
    [BsonId]
    public required DimensionId Id { get; init; }

    [Description("類型")]
    public required DimensionType Type { get; init; }  // 使用 Enum

    // 其他屬性...
}
```

### Serializer 註冊

在 `Configuration/MongoDbExtensions.cs` 中：

```csharp
using ItemBank.Database.Core.Schema.Enums;

private static void RegisterSerializers()
{
    // ... 其他註冊 ...

    // 註冊 Enum Serializer
    BsonSerializer.RegisterSerializer(new CamelCaseEnumStringSerializer<DimensionType>());

    // ... 後續程式碼 ...
}
```

### 資料庫效果

```json
{
    "_id": ObjectId("..."),
    "type": "knowledge",      // 自動轉為 camelCase
    "name": "知識向度",
    "subjectIds": ["..."],
    ...
}
```

## 註冊位置總結

所有 Serializer **必須**在 `MongoDbExtensions.cs` 的 `RegisterSerializers()` 方法中進行註冊。

**檔案位置**：`Configuration/MongoDbExtensions.cs`

**方法**：`RegisterSerializers()` 私有靜態方法

**註冊時機**：應用啟動時，於 DI 容器初始化之前

**範例**：
```csharp
public static IServiceCollection AddMongoDbContext(
    this IServiceCollection services,
    Action<MongoDbContextOptionsBuilder> configure)
{
    RegisterSerializers();  // 第一步：註冊所有 Serializer
    // ... 後續設定 ...
}
```

## 最佳實踐

### ✅ 應該做的

1. **為每個自訂類型建立專屬 Serializer**
   ```csharp
   // 好的做法
   BsonSerializer.RegisterSerializer(new DimensionTypeSerializer());
   BsonSerializer.RegisterSerializer(new UserIdSerializer());
   ```

2. **使用有意義的類名**
   ```csharp
   // 好的做法
   public class DimensionIdSerializer : NullableClassSerializerBase<DimensionId>

   // 不好的做法
   public class Serializer1 : NullableClassSerializerBase<SomeType>
   ```

3. **添加 XML 文件註解**
   ```csharp
   /// <summary>
   /// Dimension 向度 Id Serializer
   /// </summary>
   public class DimensionIdSerializer : NullableClassSerializerBase<DimensionId>
   ```

4. **在 MongoDbExtensions.cs 中按類別分組**
   ```csharp
   // ValueObject Serializers
   BsonSerializer.RegisterSerializer(new SubjectIdSerializer());
   BsonSerializer.RegisterSerializer(new DimensionIdSerializer());

   // Enum Serializers
   BsonSerializer.RegisterSerializer(new CamelCaseEnumStringSerializer<DimensionType>());
   ```

### ❌ 不應該做的

1. **在 Collection 中建立 Serializer**
   - Serializer 應集中放在 `BsonSerializers/` 目錄

2. **忘記註冊 Serializer**
   - 未註冊的 Serializer 不會生效
   - 會導致序列化錯誤或預期之外的行為

3. **在多個地方重複註冊**
   - 所有註冊應在 `RegisterSerializers()` 方法中統一進行

4. **建立過於複雜的序列化邏輯**
   - 保持 Serializer 單一職責
   - 複雜邏輯應移至 ValueObject 或業務邏輯層

## 常見問題

### Q: 何時需要 Serializer？

**A**: 當以下情況成立時：
- 需要將 Enum 轉換為字符串儲存
- ValueObject 需要轉換為原始值（String、int 等）
- 需要自訂序列化格式（如日期格式、數字精度等）

### Q: 如果沒有註冊 Serializer 會怎樣？

**A**: MongoDB 驅動程式會使用預設的序列化邏輯：
- 對象被序列化為 BSON 文件（嵌套物件）
- Enum 被序列化為整數值或字符串（取決於 BSON 驅動設定）
- 可能導致查詢和儲存的預期不符

### Q: 可以為同一類型建立多個 Serializer 嗎？

**A**: 不建議。最後註冊的 Serializer 會覆蓋之前的。應該設計單一的、全面的 Serializer。

### Q: Nullable ValueObject 如何處理？

**A**: 使用 `NullableClassSerializerBase<T>` 作為基礎類別，它已內建 null 值處理邏輯。

## 參考資源

- MongoDB BSON Serializer 文件
- 專案中的其他 Serializer 實作：`Configuration/BsonSerializers/`
- 相關 Collection 定義：`Schema/Collections/`

# 反向推導 Schema 工作流程

本文件整理本次反向推導 MongoDB schema 時實際遇到的問題與處理順序，目的是讓未來重跑時能直接取得足夠 context，而不是再從頭試錯。

## 目標

- 以 `extract-db-schema` 先取得資料庫事實。
- AI 再交叉比對現有 C# schema、value objects、serializers、enum 與 index 定義後修改 C#。
- 人員最終只需要看 `git diff` 與 `dotnet build`。

## 建議執行順序

1. 執行 `extract-db-schema` 產出 JSON 事實檔。
2. 比對資料庫 collections 與 `ItemBank.Database.Core/Schema/Collections/` 現況。
3. 先補齊高信心結構：
   - collection 類別
   - nested types
   - nullable
   - arrays / objects
   - indexes
4. 再補高信心 `Id` / enum：
   - 先沿用既有 value objects 與 enums
   - 不足者才進入唯讀查證
5. 最後處理特殊 collection 與 `Description`
6. 執行 `dotnet build "ItemBank.Database.slnx"`

## 特殊 collection 偵測規則

不是每個 collection 都適合直接建成單一平面 class。特殊情況不一定只發生在 Event collection，也可能出現在任何「同一 collection 內存在多種文件形狀」的模型。

遇到下列訊號時，必須先停下來判斷是否為「判別式模型」、「外殼模型」或一般 union / polymorphic collection：

- 存在 `_t` 欄位
- 存在 `eventType`
- 存在 `payload`
- 同時存在 `aggregate` / `aggregateId`
- 同一 collection 中，不同文件的核心欄位集合明顯依型別改變
- 同一欄位在不同文件中承載完全不同結構，而不是單純 nullable
- 某個物件欄位只在特定 `type` / `kind` / `category` / `role` / `source` 值下出現
- 某個 array 內元素形狀明顯分成多群，且以 discriminator 欄位區分

### 常見特殊模型類型

#### 1. Discriminator collection

- 特徵：通常有 `_t`、`type`、`kind` 之類欄位
- 同一 collection 中混有多個子型別
- 可能適合建成 `abstract base + derived classes`

#### 2. Envelope collection

- 特徵：外層欄位穩定，但內層 `payload` / `data` / `content` 依型別變化
- 可能適合建成 `envelope + per-type payload classes`
- Event collection 常屬於此類，但不只 Event 會出現

#### 3. Shape-shifting document collection

- 特徵：沒有明確 `_t`，但文件形狀會隨某些欄位值切換
- 常見於整合外部系統、快取資料、工作佇列、AI 結果、Webhook payload
- 這類模型未必需要做完整多型；有時保留動態型別反而較安全

### 已知需要特別處理的 collection

- `ItembankCoreEvents`
  - 現況更接近「外殼 + payload」模型，而不是一般 collection
  - `eventType` 與 `payload` 代表可能存在多種事件形狀
  - 若尚未完成 per-event payload 建模，優先採保守的動態物件 payload，不要假裝 payload 已固定
- `ImportEvents`
  - 有 `_t` 欄位，代表可能是 discriminator / subtype 模型
- `ItemBankEvents`
  - 有 `_t` 欄位，應先檢查是否是多型事件集合
- `DimensionEvents`
  - 有 `_t` 欄位，且 nested payload 也有 `_t`

### 特殊 collection 的處理原則

- 不要直接把所有欄位硬攤平成一個大 class 後就視為完成。
- 先決定要採哪種模型：
  - `abstract base + derived classes`
  - `event envelope + per-event payload classes`
  - `general envelope + per-type payload classes`
  - `保留動態型別的保守模型`
- 若資料證據不足，至少要在文件中標記此 collection 為「特殊模型候選」，避免未來 AI 直接套用一般規則。
- 若目前先保守落地，可先保留 envelope class，但必須記錄這不是最終理想建模。
- 若 collection 並非 Event，但仍有明顯多型特徵，也應套用相同判斷流程，不可因名稱不像 Event 就直接視為一般 collection。

## Description 補齊規則

本專案既有 schema 大量使用 `[Description("...")]`。反向推導後若缺少 `Description`，代表結果仍不完整。

### 補齊順序

1. collection class 的 `[Description]`
2. property 的 `[Description]`
3. nested class 的 `[Description]`

### Description 來源優先序

1. 現有主專案相近 collection / nested type 的命名與描述
2. `itembank-database-query-with-ai/docs/itembank-schema.yaml` 中既有欄位描述
3. 由欄位名稱保守翻譯出的描述

### 注意

- `Description` 屬低信心資訊，不可僅憑想像寫過度業務化的說明。
- 若只能保守命名，應採用簡單直譯，例如：
  - `CreatedAt` -> `建立時間`
  - `UpdatedBy` -> `更新者`
  - `Status` -> `狀態`
- 若是特殊 collection 的 payload 欄位，先保守描述，不要假設完整事件語意。

## 動態型別保留規則

不是所有動態型別都代表漏改。有些欄位本來就應保守保留。

### 可考慮再收斂的情況

- 查證後發現實際只有單一 primitive 型別
- 幾乎都是 `null`，但少數非 null 樣本能明確看出固定型別
- 值集合穩定到可以安全轉為 enum / bool / string / id

### 應保留動態型別的情況

- heterogeneous 結構
- 自由格式附加資料
- 抽樣幾乎都是空陣列 / `null` / 缺欄位，且沒有足夠非 null 證據

## Enum / Id 查證規則

### Enum

- 先查 distinct values + counts
- 檢查空字串、`null`、大小寫混用、離群值
- 若值集合穩定，才考慮建立 enum
- 若資料含髒值，先評估是否需要清理或容錯，不能直接硬轉 enum

### Id

- 先查 BSON 型別
- 再查值樣貌、長度、格式
- 必要時查對目標 collection `_id` 的命中率
- 只有當它是穩定、明確的領域識別碼時，才值得新增 value object

## 本次已證實可重用的經驗

- `AIGeneration.Tasks.Status` 可建立專屬 enum
- `AIGeneration.GeneratedItems.Status` 可建立專屬 enum
- `AIGeneration.GeneratedItems.WordExportStatus` 可建立專屬 enum
- `AIGeneration.GeneratedImages.ImageType` 可建立專屬 enum
- `AIGeneration.Prompts.Type` 可建立專屬 enum
- `ChatBot.AiResultCaches.Type` 可建立專屬 enum
- `ItemImage.ProcessingRecords.EmbeddingStatus` / `GeminiStatus` 可共用工作狀態 enum
- `ItemImage.DuplicateCheckJobs.DuplicateCheckStatus` 可共用工作狀態 enum
- `Search.Items.OnlineReadiness` 值集合穩定，可建立 snake_case enum
- `Search.Queries.QueryInclude.VideoLink` 可收斂成 `bool?`
- `ContentSections.Description` 可由動態型別收斂成 `string?`
- `EmailQueues.ErrorMessage` 可由動態型別收斂成 `string?`

## 本次仍需保守保留的項目

- `Search.Items.Copyright`
- `ChatBot.Documents.Status`
- `ChatBot.RawSections.Status`
- `ChatBot.RawSections.Type`
- `AIGeneration.Tasks.TaskType`
- `AIGeneration.GeneratedItems.ItemType`
- `AIGeneration.GeneratedItems.ContextType`
- `AIGeneration.ItemShells.ItemType`
- `AIGeneration.ItemShells.Shell.Type`
- `ItemMapping.PreSelectedItemMembers.GroupingId`
- `ChatBot.Sessions.SessionId`
- `ItemImage.*.BatchId`
- `ItembankCoreEvents` 整體建模方式

## 最後檢查清單

- [ ] 已執行 `extract-db-schema`
- [ ] 已檢查是否存在特殊 collection（`_t` / `eventType` / `payload`）
- [ ] 已補齊高信心結構
- [ ] 已補齊高信心 value objects / enums
- [ ] 已為新增 schema 補上 `Description`
- [ ] 已記錄仍需唯讀查證的欄位
- [ ] 已執行 `dotnet build "ItemBank.Database.slnx"`

# ItemBank Database 主專案指引

## 專案定位

- 本專案是 ItemBank 資料庫 C# Schema 與相關工具的權威來源。
- `itembank-database-query-with-ai/` 是 schema 使用端與 AI 查詢腳本執行環境，不是主專案 schema 的人工維護位置。

## 工作分流

- 若任務是新增、修正、比對或反向推導 MongoDB Schema，請在本專案中完成。
- 若任務是撰寫唯讀查詢腳本、產出查詢報表或協助使用者查資料，請進入 `itembank-database-query-with-ai/`，並先閱讀 `itembank-database-query-with-ai/AGENTS.md` 與其 `docs/` 指引。
- `itembank-database-query-with-ai/` 僅可作為查詢執行環境與參考資料來源；除非使用者明確要求，否則不得修改、建立或提交該 submodule 內的任何檔案。

## 反向推導 Schema 的工作流程

當需求是「依既有資料庫內容補齊或修正 C# schema」時，請遵循以下流程：

1. 先使用主專案提供的 schema 擷取工具輸出 JSON 事實檔（預期命令名稱為 `extract-db-schema`）。
2. AI 先閱讀該 JSON 輸出，再閱讀目前的 C# schema、value objects、serializers 與索引定義。
3. 若需要判斷 enum、強型別 Id 或其他低信心資訊，AI 應進入 `itembank-database-query-with-ai/` 撰寫唯讀查詢腳本蒐集證據，再回到主專案修改 C#。
4. AI 根據「資料庫事實 + 唯讀查詢證據 + 現有 C#」交叉比對後再修改 C#。
5. 人員透過 `git diff` 審查最終 C# 變更。

詳細工作順序、特殊 collection 判斷規則、`Description` 補齊規則與動態型別保留原則，請同步閱讀 `docs/REVERSE_SCHEMA_WORKFLOW.md`。

## 重要原則

- 工具只負責擷取資料庫事實，不直接修改 C#。
- 不可跳過 JSON 事實檔，直接憑資料庫樣本或直覺修改 `ItemBank.Database.Core`。
- 優先沿用既有專案模式，例如：
  - `CollectionNameAttribute`
  - `IIndexable<T>`
  - 強型別 Id value objects
  - 既有 BSON serializer 與註冊方式
- 反向推導後，不可只補結構而忽略 `[Description]`；新增 collection、nested type 與 property 應依既有專案模式補上描述。
- 遇到 `_t`、`eventType`、`payload`、`aggregate`、`aggregateId`，或同一 collection 內文件形狀明顯依某欄位切換時，必須先判斷是否為特殊 collection / 多型模型，不可直接套用一般 collection 規則。

## 高低信心規則

### 高信心，可保守調整

- 欄位是否存在
- nullable 與 required 差異
- 巢狀物件結構
- array/object/primitive 型別
- MongoDB index 定義

### 低信心，需保守判斷

- `Description` 文字
- enum 的建立與命名
- 強型別 Id 命名與底層型別推論
- `IAuditable`、`IFinalizable` 等業務介面
- 特殊 collection 的 discriminator / envelope / 多型建模方式

若低信心資訊無法由現有專案脈絡確認，應先保守處理，避免過度推論。

## 低信心資訊的查證方式

- `extract-db-schema` 主要用於結構事實，例如欄位、nullable、巢狀物件、array 與索引。
- 若要判斷 enum 是否應建立，應到 `itembank-database-query-with-ai/` 查詢目標欄位的 distinct values、筆數分布、null/空字串/離群值。
- 若要判斷強型別 Id 是否應建立，應到 `itembank-database-query-with-ai/` 查詢欄位值的 BSON 型別、實際值樣貌，以及對目標 collection `_id` 的命中率。
- 查證腳本必須維持唯讀，不可在主專案內直接加入臨時查詢程式或暫存腳本。
- 若本次任務未明確要求修改 submodule，則只可使用其既有工具與環境，不得異動 `itembank-database-query-with-ai/` 內檔案。

## 修改前後檢查

- 修改前先閱讀相關 collection、nested types、value objects、serializers 與 generator 用法。
- 修改後至少執行 `dotnet build "ItemBank.Database.slnx"`。
- 不要主動 commit。

## 與 AI 查詢專案的關係

- 若後續需要提供 AI 查詢使用的 schema，應由本專案工具或流程產出，再同步到 `itembank-database-query-with-ai/`。
- 不要將 `itembank-database-query-with-ai/` 視為 schema 權威來源來回寫主專案。

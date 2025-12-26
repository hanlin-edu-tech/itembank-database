using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemBank.Database.Core.Schema.Collections;

[Description("匯入任務")]
public class ImportTask
{
    [BsonId]
    [Description("Id")]
    public required ObjectId TaskId { get; init; }

    [Description("項目清單")]
    public required List<ImportTaskItem> Items { get; init; }

    [Description("匯入任務項目")]
    public class ImportTaskItem
    {
        [Description("Id")]
        public required string Id { get; init; }
    }
}

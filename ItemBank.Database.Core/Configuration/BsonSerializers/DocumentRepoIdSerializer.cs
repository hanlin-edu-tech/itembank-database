using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 五欄檔案儲存庫 Id 序列化器
/// </summary>
public class DocumentRepoIdSerializer : NullableClassSerializerBase<DocumentRepoId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 DocumentRepoId
    /// </summary>
    protected override DocumentRepoId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new DocumentRepoId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 DocumentRepoId");
    }

    /// <summary>
    /// 序列化 DocumentRepoId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, DocumentRepoId value)
    {
        context.Writer.WriteString(value.Value);
    }
}

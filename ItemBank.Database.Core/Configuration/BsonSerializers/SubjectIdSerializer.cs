using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// 科目 Id 序列化器
/// </summary>
public class SubjectIdSerializer : NullableClassSerializerBase<SubjectId>
{
    /// <summary>
    /// 反序列化 BSON 字串為 SubjectId
    /// </summary>
    protected override SubjectId DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args, BsonType bsonType)
    {
        if (bsonType == BsonType.String)
            return new SubjectId(context.Reader.ReadString());
        throw new BsonSerializationException($"無法從 {bsonType} 反序列化 SubjectId");
    }

    /// <summary>
    /// 序列化 SubjectId 為 BSON 字串
    /// </summary>
    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, SubjectId value)
    {
        context.Writer.WriteString(value.Value);
    }
}

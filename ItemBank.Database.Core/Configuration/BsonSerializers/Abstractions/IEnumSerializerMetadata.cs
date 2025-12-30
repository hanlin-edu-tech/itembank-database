namespace ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;

/// <summary>
/// Enum 序列化器的元數據介面
/// </summary>
public interface IEnumSerializerMetadata
{
    /// <summary>
    /// 序列化類型
    /// </summary>
    EnumSerializationType SerializationType { get; }

    /// <summary>
    /// 所有 Enum 值的序列化結果（name -> serialized value）
    /// </summary>
    IReadOnlyDictionary<string, string> SerializedValues { get; }
}

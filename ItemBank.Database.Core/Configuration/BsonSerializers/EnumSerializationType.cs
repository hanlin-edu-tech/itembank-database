namespace ItemBank.Database.Core.Configuration.BsonSerializers;

/// <summary>
/// Enum 序列化類型
/// </summary>
public enum EnumSerializationType
{
    /// <summary>
    /// 序列化為 camelCase 字串
    /// </summary>
    CamelCase,

    /// <summary>
    /// 序列化為 PascalCase 字串（保持原樣）
    /// </summary>
    PascalCase,

    /// <summary>
    /// 序列化為整數
    /// </summary>
    Integer
}

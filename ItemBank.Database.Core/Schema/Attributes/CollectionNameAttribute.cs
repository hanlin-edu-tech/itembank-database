namespace ItemBank.Database.Core.Schema.Attributes;

/// <summary>
/// 指定 MongoDB 集合的名稱
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class CollectionNameAttribute(string name) : Attribute
{
    /// <summary>
    /// MongoDB 集合的名稱
    /// </summary>
    public string Name { get; } = !string.IsNullOrWhiteSpace(name)
        ? name
        : throw new ArgumentException("集合名稱不可為空", nameof(name));
}

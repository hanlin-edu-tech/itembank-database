namespace ItemBank.Database.Core.Schema.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class CollectionSchemaDiscriminatorAttribute(string fieldName) : Attribute
{
    public string FieldName { get; } = !string.IsNullOrWhiteSpace(fieldName)
        ? fieldName
        : throw new ArgumentException("欄位名稱不可為空", nameof(fieldName));
}

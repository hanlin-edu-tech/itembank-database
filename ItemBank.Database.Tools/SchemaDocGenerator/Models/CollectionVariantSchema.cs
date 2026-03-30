namespace ItemBank.Database.Tools.SchemaDocGenerator.Models;

public sealed record CollectionVariantSchema
{
    public required string Name { get; init; }

    public required string TypeName { get; init; }

    public required string Description { get; init; }

    public string? DiscriminatorValue { get; init; }
}

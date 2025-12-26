namespace ItemBank.Database.Core.Schema.Interfaces;

/// <summary>
/// Represents an entity with audit information.
/// </summary>
public interface IAuditable
{
    /// <summary>Gets the user who created this entity.</summary>
    string CreatedBy { get; }

    /// <summary>Gets the date when this entity was created.</summary>
    DateTime CreatedOn { get; }

    /// <summary>Gets the user who last updated this entity.</summary>
    string UpdatedBy { get; }

    /// <summary>Gets the date when this entity was last updated.</summary>
    DateTime UpdatedOn { get; }
}

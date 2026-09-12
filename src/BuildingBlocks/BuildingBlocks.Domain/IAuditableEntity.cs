namespace BuildingBlocks.Domain;

/// <summary>
/// Interface for entities that track creation and modification auditing metadata.
/// </summary>
public interface IAuditableEntity
{
    DateTime CreatedAtUtc { get; set; }
    string? CreatedBy { get; set; }
    DateTime? LastModifiedAtUtc { get; set; }
    string? LastModifiedBy { get; set; }
}

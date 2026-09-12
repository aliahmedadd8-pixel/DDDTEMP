namespace BuildingBlocks.Domain;

/// <summary>
/// Interface for entities supporting soft-deletion.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAtUtc { get; set; }
}

namespace BuildingBlocks.Domain;

/// <summary>
/// Marker interface for Domain Events representing something that happened in the domain.
/// </summary>
public interface IDomainEvent
{
    Guid EventId => Guid.NewGuid();
    DateTime OccurredOnUtc => DateTime.UtcNow;
}

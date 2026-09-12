using BuildingBlocks.Domain;

namespace BuildingBlocks.Application;

/// <summary>
/// Handler interface for Domain Events within the same bounded context or application.
/// </summary>
public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

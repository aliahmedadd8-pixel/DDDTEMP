using MediatR;

namespace BuildingBlocks.Application;

/// <summary>
/// Represents an event that crosses Module (Bounded Context) boundaries.
/// Implements INotification to allow in-process decoupled dispatching.
/// </summary>
public interface IIntegrationEvent : INotification
{
    Guid Id => Guid.NewGuid();
    DateTime OccurredOnUtc => DateTime.UtcNow;
}

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default)
        where TEvent : class, IIntegrationEvent;
}

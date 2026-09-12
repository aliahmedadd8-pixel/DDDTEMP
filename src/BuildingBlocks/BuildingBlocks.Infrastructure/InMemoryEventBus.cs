using BuildingBlocks.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure;

public sealed class InMemoryEventBus(IPublisher publisher, ILogger<InMemoryEventBus> logger) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default)
        where TEvent : class, IIntegrationEvent
    {
        logger.LogInformation(
            "Publishing integration event {EventName} with ID {EventId}",
            typeof(TEvent).Name,
            integrationEvent.Id);

        await publisher.Publish(integrationEvent, cancellationToken);
    }
}

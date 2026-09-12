using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Interceptors;

public sealed class DomainEventsDispatcherInterceptor(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<DomainEventsDispatcherInterceptor> logger) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var entitiesWithEvents = context.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        if (entitiesWithEvents.Count == 0)
        {
            return;
        }

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // Clear events so they are not dispatched twice
        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        using var scope = serviceScopeFactory.CreateScope();

        foreach (var domainEvent in domainEvents)
        {
            var eventType = domainEvent.GetType();
            logger.LogInformation("Dispatching Domain Event: {EventName}", eventType.Name);

            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
            var handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null) continue;

                var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle));
                if (method is not null)
                {
                    var task = (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
                    await task;
                }
            }
        }
    }
}

using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IEventBus, InMemoryEventBus>();
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DomainEventsDispatcherInterceptor>();

        return services;
    }
}

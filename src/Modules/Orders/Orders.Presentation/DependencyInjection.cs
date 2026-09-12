using Microsoft.Extensions.DependencyInjection;
using Orders.Application;

namespace Orders.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersPresentation(this IServiceCollection services)
    {
        services.AddOrdersApplication();
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Users.Application;

namespace Users.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersPresentation(this IServiceCollection services)
    {
        services.AddUsersApplication();
        return services;
    }
}

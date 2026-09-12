using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksPresentation(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WeightControl.Core;

public static class CoreServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
    {
        //services.AddScoped<IDeleteContributorService, DeleteContributorService>();

        logger.LogInformation("{Project} services registered", "Core");

        return services;
    }
}

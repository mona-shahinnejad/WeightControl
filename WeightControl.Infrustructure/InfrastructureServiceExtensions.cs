using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeightControl.Application.Products.FilterByWeight;
using WeightControl.Application.Products.List;
using WeightControl.Application.Products.WeightToleranceCheck;
using WeightControl.Infrustructure.Data;
using WeightControl.Infrustructure.Data.Queries;

namespace WeightControl.Infrustructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      ILogger logger)
    {
        services.AddScoped<IListProductsQueryService, ListProductsQueryService>();
        services.AddScoped<IWeightToleranceCheckQueryService, WeightToleranceCheckQueryService>();
        services.AddScoped<IFilterByWeightService, FilterByWeightService>();

        RegisterEF(services);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

    private static void RegisterEF(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    }
}

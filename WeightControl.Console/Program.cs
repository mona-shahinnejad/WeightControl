using Ardalis.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using WeightControl.Application.Products.Create;
using WeightControl.Console;
using WeightControl.Core;
using WeightControl.Infrustructure;
using WeightControl.Infrustructure.Data;

var host = Host.CreateDefaultBuilder()
               .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                })
               .ConfigureAppConfiguration((context, config) =>
                 {
                     // Add the appsettings.json configuration file
                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("SqliteConnection");
                    services.AddDbContext<AppDbContext>(options =>
                             options.UseSqlite(connectionString));

                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                        // Use the extension method to add services with logging
                        services.AddCoreServices(logger);
                        services.AddInfrastructureServices(logger);

                        var mediatRAssemblies = new[]
                        {
                        Assembly.GetAssembly(typeof(CoreServiceExtensions)), // Core
                        Assembly.GetAssembly(typeof(CreateProductCommand)), // Application
                        Assembly.GetAssembly(typeof(InfrastructureServiceExtensions)) // Infrastructure
                        };

                        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
                        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

                        services.AddScoped<WeightController>();
                    }
                }).Build();

SeedDatabase(host);

var weightControl = host.Services.GetRequiredService<WeightController>();
weightControl.RunApplication();


void SeedDatabase(IHost host)
{
    try
    {
        var context = host.Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        SeedData.Initialize(host.Services);
    }
    catch (Exception ex)
    {
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
}


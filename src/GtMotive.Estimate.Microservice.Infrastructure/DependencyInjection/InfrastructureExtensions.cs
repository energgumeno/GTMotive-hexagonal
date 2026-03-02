using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Infrastructure.ServiceBus;
using GtMotive.Estimate.Microservice.Infrastructure.Telemetry;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Settings
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
        services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBus"));

        // Persistence
        services.AddSingleton<MongoService>();
        services.AddScoped<IVehiclePort, MongoVehicleAdapter>();
        services.AddScoped<IRentVehiclePort, MongoRentVehicleAdapter>();

        // Service Bus
        services.AddSingleton<AzureBusFactory>();
        services.AddSingleton<IBusFactory>(sp => sp.GetRequiredService<AzureBusFactory>());
        services.AddScoped<IBus>(sp => sp.GetRequiredService<IBusFactory>().GetClient(typeof(object)));

        // Logging
        services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        // Telemetry
        // Injects TelemetryClient as null if not registered by the host
        services.AddSingleton<ITelemetry>(sp =>
        {
            var telemetryClient = sp.GetService<TelemetryClient>();
            return new AppTelemetry(telemetryClient);
        });

        return services;
    }
}
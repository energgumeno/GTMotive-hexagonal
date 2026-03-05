using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Api.UseCases.Common.BaseHandler;
using GtMotive.Estimate.Microservice.ApplicationCore.DependencyInjection;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection;

public static class UserInterfaceExtensions
{
    public static IServiceCollection AddPresenters(this IServiceCollection services, IConfiguration configuration)
    {
        // infrastructure & application core
        services.AddInfrastructure(configuration);
        services.AddApplicationCore();

        // web api
        services.AddScoped<IWebApiPresenter, WebApiPresenter>();

        // output ports
        services.AddScoped<IErrorOutputPort, ErrorOutputPort>();
        services.AddScoped(typeof(IOutputPortStandard<>), typeof(BaseHandler<>));

        return services;
    }
}
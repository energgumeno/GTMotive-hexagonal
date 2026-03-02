using GtMotive.Estimate.Microservice.ApplicationCore.Common.NullObjects;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.ApplicationCore.DependencyInjection;

public static class ApplicationCoreExtensions
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        // use cases
        services.AddScoped<IUseCase<AddVehicleCommand>, AddVehicleCase>();
        services.AddScoped<IUseCase<ListVehicleCommand>, ListVehicleCase>();
        services.AddScoped<IUseCase<RentVehicleCommand>, RentVehicleCase>();
        services.AddScoped<IUseCase<ReturnVehicleCommand>, ReturnVehicleCase>();
        services.AddScoped<IUseCase<ProcessRentCreatedCommand>, ProcessRentCreatedCase>();
        services.AddScoped<IUseCase<ProcessRentReturnedCommand>, ProcessRentReturnedCase>();

        // default output ports (null implementation)
        // these can be overridden by specific UI extensions (e.g. WebApi)
        services.AddScoped<IOutputPortNotFound, NullOutputPortNotFound>();
        services.AddScoped(typeof(IOutputPortStandard<>), typeof(NullOutputPortStandard<>));

        return services;
    }
}
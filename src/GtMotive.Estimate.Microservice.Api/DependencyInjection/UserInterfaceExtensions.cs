using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.ServiceBus;
using GtMotive.Estimate.Microservice.Infrastructure.Telemetry;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            
            //web api
            services.AddScoped<IWebApiPresenter, WebApiPresenter>();
            services.AddScoped<IWebApiSetter, WebApiPresenter>();
            //use cases
            services.AddScoped<IUseCase<AddVehicleCommand>, AddVehicleCase>();
            services.AddScoped<IUseCase<ListVehicleCommand>, ListVehicleCase>();
            services.AddScoped<IUseCase<RentVehicleCommand>, RentVehicleCase>();
            services.AddScoped<IUseCase<ReturnVehicleCommand>, ReturnVehicleCase>();
            services.AddScoped<IUseCase<ProcessRentCreatedCommand>, ProcessRentCreatedCase>();
            services.AddScoped<IUseCase<ProcessRentReturnedCommand>, ProcessRentReturnedCase>();
            //output ports
            services.AddScoped<IOutputPortNotFound, OutputPortNotFound>();
            services.AddScoped(typeof(IOutputPortStandard<>), typeof(BaseHandler<>));
            //persistence
            services.AddSingleton<MongoService>();
            services.AddScoped<IVehiclePort, MongoVehicleAdapter>();
            services.AddScoped<IRentVehiclePort, MongoRentVehicleAdapter>();
            //infrastructure
            services.AddSingleton<IBusFactory, AzureBusFactory>();
            services.AddScoped<IBus>(sp => sp.GetRequiredService<IBusFactory>().GetClient(typeof(object)));
            //logging
            services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            //telemetry
            services.AddSingleton<ITelemetry, AppTelemetry>();

            return services;
        }
    }
}

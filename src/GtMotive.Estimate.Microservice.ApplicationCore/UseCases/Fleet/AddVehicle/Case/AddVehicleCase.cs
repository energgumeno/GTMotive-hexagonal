using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Events;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Case;

/// <summary>
///     A Handler for a vehicle creation.
/// </summary>
/// <param name="vehiclePort">The vehicle port.</param>
/// <param name="busFactory">The bus factory.</param>
/// <param name="telemetry">The telemetry.</param>
/// <param name="logger">The app logger.</param>
/// <param name="outputPortStandard">The standard output port.</param>
/// <param name="errorOutputPort">The error output port.</param>
public class AddVehicleCase(
    IVehiclePort vehiclePort,
    IBusFactory busFactory,
    ITelemetry telemetry,
    IAppLogger<AddVehicleCase> logger,
    IOutputPortStandard<AddVehicleResponse> outputPortStandard,
    IErrorOutputPort errorOutputPort) : IUseCase<AddVehicleCommand>
{
    /// <summary>
    ///     adds a vehicle to the collection
    /// </summary>
    /// <param name="request"> the vehicle.</param>
    /// <exception cref="ArgumentException">argument exception if the input is not set.</exception>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task Execute(AddVehicleCommand request)
    {
        try
        {
            var bus = busFactory.GetClient(typeof(VehicleCreatedEvent));


            telemetry.TrackEvent(nameof(AddVehicleCase),
                new Dictionary<string, string> { { "AddVehicleCase", "Start..." } });

            var existingVehicle = await vehiclePort.GetVehicle(vehicle =>
                vehicle.FrameId == request.FrameId
            );

            var vehicleAggregate = VehicleAggregate.Create(
                request.RegistrationDate,
                request.FrameId!,
                request.LicensePlate!, existingVehicle);


            await vehiclePort.AddVehicle(vehicleAggregate.CurrentVehicle!)!;
            await vehiclePort.Save();

            foreach (var vehicleAggregateDomainEvent in vehicleAggregate.DomainEvents)
                await bus.Send(vehicleAggregateDomainEvent);


            telemetry.TrackEvent(nameof(AddVehicleCase),
                new Dictionary<string, string> { { "AddVehicleCase", "End..." } });
            outputPortStandard.StandardHandle(new AddVehicleResponse(vehicleAggregate.CurrentVehicle!.Id));
        }
        catch (ArgumentException ex)
        {
            errorOutputPort.BadRequestHandle(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            errorOutputPort.BadRequestHandle(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while executing AddVehicleCase.");
            errorOutputPort.GeneralErrorHandle("An unexpected error occurred. Please try again later.");
        }
    }
}
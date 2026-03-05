using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Events;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Case;

/// <summary>
///     Use case for renting a vehicle.
/// </summary>
/// <param name="vehiclePort">Port for vehicle operations.</param>
/// <param name="rentVehiclePort">Port for rent vehicle operations.</param>
/// <param name="busFactory">Factory for bus clients.</param>
/// <param name="telemetry">Telemetry service.</param>
/// <param name="logger">The app logger.</param>
/// <param name="outputPortStandard">Standard output port.</param>
/// <param name="errorOutputPort">Error output port.</param>
public class RentVehicleCase(
    IVehiclePort vehiclePort,
    IRentVehiclePort rentVehiclePort,
    IBusFactory busFactory,
    ITelemetry telemetry,
    IAppLogger<RentVehicleCase> logger,
    IOutputPortStandard<RentVehicleResponse> outputPortStandard,
    IErrorOutputPort errorOutputPort) : IUseCase<RentVehicleCommand>
{
    /// <summary>
    ///     Executes the rent vehicle use case.
    /// </summary>
    /// <param name="request">The rent vehicle command.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Execute(RentVehicleCommand request)
    {
        try
        {
            telemetry.TrackEvent(nameof(RentVehicleCase),
                new Dictionary<string, string> { { nameof(RentVehicleCase), "Start..." } });

            var vehicle = await vehiclePort.GetVehicle(vehicle => vehicle.Id == request.VehicleId!.Value);
            var rentVehicleByEmail =
                await rentVehiclePort.GetVehiclesRent(information => information.Email.ToLower().Trim() == request.Email!.ToLower().Trim());
            var reservations =
                await rentVehiclePort.GetVehiclesRent(information => information.VehicleId == request.VehicleId!.Value);

            var vehicleRentAggregate = VehicleRentAggregate.Create(
                request.Fullname!,
                request.Email!,
                request.TimeRentStart!.Value,
                request.TimeRentEnd!.Value,
                vehicle,
                rentVehicleByEmail,
                reservations);

            await rentVehiclePort.AddVehicleRent(vehicleRentAggregate.RentVehicleInformation!);

            var bus = busFactory.GetClient(typeof(RentVehicleCreatedEvent));
            foreach (var vehicleRentAggregateDomainEvent in vehicleRentAggregate.DomainEvents)
                await bus.Send(vehicleRentAggregateDomainEvent);

            telemetry.TrackEvent(nameof(RentVehicleCase),
                new Dictionary<string, string> { { nameof(RentVehicleCase), "End..." } });


            outputPortStandard.StandardHandle(new RentVehicleResponse(vehicleRentAggregate.RentVehicleInformation!.Id));
        }
        catch (ArgumentException ex)
        {
            errorOutputPort.BadRequestHandle(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                errorOutputPort.NotFoundHandle(ex.Message);
            }
            else
            {
                errorOutputPort.BadRequestHandle(ex.Message);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while executing RentVehicleCase.");
            errorOutputPort.GeneralErrorHandle("An unexpected error occurred. Please try again later.");
        }
    }
}
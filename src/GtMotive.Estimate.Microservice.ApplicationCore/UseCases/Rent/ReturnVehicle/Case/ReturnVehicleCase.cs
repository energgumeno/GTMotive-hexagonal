using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Case;

/// <summary>
///     Use case for returning a vehicle.
/// </summary>
/// <param name="busFactory">Factory for bus clients.</param>
/// <param name="telemetry">Telemetry service.</param>
/// <param name="logger">The app logger.</param>
/// <param name="rentVehiclePort">Port for rent vehicle operations.</param>
/// <param name="outputPortStandard">Standard output port.</param>
/// <param name="errorOutputPort">Error output port.</param>
public class ReturnVehicleCase(
    IBusFactory busFactory,
    ITelemetry telemetry,
    IAppLogger<ReturnVehicleCase> logger,
    IRentVehiclePort rentVehiclePort,
    IOutputPortStandard<ReturnVehicleResponse> outputPortStandard,
    IErrorOutputPort errorOutputPort)
    : IUseCase<ReturnVehicleCommand>
{
    /// <summary>
    ///     Executes the return vehicle use case.
    /// </summary>
    /// <param name="request">The return vehicle command.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Execute(ReturnVehicleCommand request)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);
            if (!request.RentId.HasValue) throw new ArgumentNullException(nameof(request.RentId));

            var bus = busFactory.GetClient(typeof(ReturnVehicleCase));


            telemetry.TrackEvent(nameof(ReturnVehicleCommand),
                new Dictionary<string, string> { { nameof(ReturnVehicleCommand), "Start..." } });

            var vehicleRent =
                await rentVehiclePort.GetVehicleRent(information => information.Id == request.RentId.Value);
            if (vehicleRent == null)
            {
                errorOutputPort.NotFoundHandle($"Vehicle rent with id {request.RentId.Value} not found");
                return;
            }


            var vehicleRentAggregate = VehicleRentAggregate.ReturnVehicle(vehicleRent);
            await rentVehiclePort.UpdateVehicleRent(vehicleRentAggregate.RentVehicleInformation!);

            foreach (var vehicleRentAggregateDomainEvent in vehicleRentAggregate.DomainEvents)
                await bus.Send(vehicleRentAggregateDomainEvent);

            telemetry.TrackEvent(nameof(ReturnVehicleCommand),
                new Dictionary<string, string> { { nameof(ReturnVehicleCommand), "End..." } });


            outputPortStandard.StandardHandle(new ReturnVehicleResponse(request.RentId.Value));
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
            logger.LogError(ex, "An unexpected error occurred while executing ReturnVehicleCase.");
            errorOutputPort.GeneralErrorHandle("An unexpected error occurred. Please try again later.");
        }
    }
}
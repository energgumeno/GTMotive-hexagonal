using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Events;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Case;

/// <summary>
///     A Handler for a vehicle creation.
/// </summary>
public class AddVehicleCase(
    IVehiclePort vehiclePort,
    IBusFactory busFactory,
    ITelemetry telemetry,
    IOutputPortStandard<AddVehicleResponse> outputPortStandard,
    IOutputPortNotFound outputPortNotFound) : IUseCase<AddVehicleCommand>
{
    /// <summary>
    ///     adds a vehicle to the collection
    /// </summary>
    /// <param name="request"> the vehicle.</param>
    /// <exception cref="ArgumentException">argument exception if the input is not set.</exception>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task Execute(AddVehicleCommand request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.RegistrationDate == null) throw new ArgumentException("Registration date is required");

        if (request.FrameId == null) throw new ArgumentException("Frame id is required");

        if (request.LicensePlate == null) throw new ArgumentException("License Plate id is required");

        var bus = busFactory.GetClient(typeof(VehicleCreatedEvent));


        telemetry.TrackEvent(nameof(AddVehicleCase),
            new Dictionary<string, string> { { "AddVehicleCase", "Start..." } });

        var existingVehicle = await vehiclePort.GetVehicle(vehicle =>
            vehicle.RegistrationDate == request.RegistrationDate &&
            vehicle.FrameId == request.FrameId &&
            vehicle.LicensePlate == request.LicensePlate);

        var vehicleAggregate = VehicleAggregate.Create(
            request.RegistrationDate,
            request.FrameId,
            request.LicensePlate, existingVehicle);


        await vehiclePort.AddVehicle(vehicleAggregate.CurrentVehicle!)!;
        await vehiclePort.Save();

        foreach (var vehicleAggregateDomainEvent in vehicleAggregate.DomainEvents)
            await bus.Send(vehicleAggregateDomainEvent);


        telemetry.TrackEvent(nameof(AddVehicleCase),
            new Dictionary<string, string> { { "AddVehicleCase", "End..." } });
        outputPortStandard.StandardHandle(new AddVehicleResponse(vehicleAggregate.CurrentVehicle!.Id));
    }
}
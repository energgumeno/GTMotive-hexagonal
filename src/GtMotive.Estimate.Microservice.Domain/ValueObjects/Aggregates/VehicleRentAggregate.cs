using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

public class VehicleRentAggregate : EntityBase
{
    public RentInformation? RentVehicleInformation { get; private set; }

    public static VehicleRentAggregate Create(
        string fullname,
        string? email,
        DateTime? timeRentStart,
        DateTime? timeRentEnd,
        Guid? vehicleId)
    {
        var vehicleRentAggregate = new VehicleRentAggregate();
        vehicleRentAggregate.RentVehicleInformation = RentInformation.Create(
            fullname,
            email,
            timeRentStart,
            timeRentEnd,
            vehicleId);
        vehicleRentAggregate.AddDomainEvent(
            new RentVehicleCreatedEvent(vehicleRentAggregate.RentVehicleInformation));
        return vehicleRentAggregate;
    }

    public static VehicleRentAggregate AcceptRent(RentInformation rentInformation)
    {
        var aggregate = new VehicleRentAggregate { RentVehicleInformation = rentInformation };
        aggregate.RentVehicleInformation.Accept();
        // aggregate.AddDomainEvent(new RentVehicleAcceptedEvent(rentInformation)); // Opcional si se requiere un evento de salida
        return aggregate;
    }

    public static VehicleRentAggregate CancelRent(RentInformation rentInformation)
    {
        var aggregate = new VehicleRentAggregate { RentVehicleInformation = rentInformation };
        aggregate.RentVehicleInformation.Cancel();
        return aggregate;
    }

    public static VehicleRentAggregate ReturnVehicle(RentInformation rentInformation)
    {
        var vehicleRentAggregate = new VehicleRentAggregate { RentVehicleInformation = rentInformation };

        vehicleRentAggregate.RentVehicleInformation.ReturnVehicle();
        vehicleRentAggregate.AddDomainEvent(new RentVehicleReturnedEvent(rentInformation));
        return vehicleRentAggregate;
    }
}
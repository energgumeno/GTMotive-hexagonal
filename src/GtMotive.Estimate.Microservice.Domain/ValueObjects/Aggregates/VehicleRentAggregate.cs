using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates
{
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
            VehicleRentAggregate vehicleRentAggregate = new VehicleRentAggregate();
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

        public static VehicleRentAggregate ReturnVehicle(RentInformation rentInformation)
        {
            VehicleRentAggregate vehicleRentAggregate = new VehicleRentAggregate();

            rentInformation.ReturnVehicle();
            vehicleRentAggregate.AddDomainEvent(new RentVehicleReturnedEvent(rentInformation));
            return vehicleRentAggregate;
        }
    }
}
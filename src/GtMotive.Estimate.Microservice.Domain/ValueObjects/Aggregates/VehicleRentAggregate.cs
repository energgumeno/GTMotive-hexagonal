using GtMotive.Estimate.Microservice.Domain.Common;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates
{
    public class VehicleRentAggregate : EntityBase
    {
        public RentInformation RentVehicleInformation { get; private set; }

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
            return vehicleRentAggregate;
        }
    }
}
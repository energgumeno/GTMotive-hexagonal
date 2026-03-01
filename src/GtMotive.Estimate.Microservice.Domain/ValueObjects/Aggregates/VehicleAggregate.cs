using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates
{
    public class VehicleAggregate : EntityBase
    {
        public Vehicle? CurrentVehicle { get; private set; }

        public static VehicleAggregate Create(
            DateTime? inputRegistrationDate,
            string inputFrameId,
            string inputLicensePlate)
        {
            VehicleAggregate vehicleAggregate = new VehicleAggregate();
            vehicleAggregate.CurrentVehicle = Vehicle.Create(inputRegistrationDate, inputFrameId, inputLicensePlate);
            vehicleAggregate.AddDomainEvent(new VehicleCreatedEvent(vehicleAggregate.CurrentVehicle));
            return vehicleAggregate;
        }
    }
}
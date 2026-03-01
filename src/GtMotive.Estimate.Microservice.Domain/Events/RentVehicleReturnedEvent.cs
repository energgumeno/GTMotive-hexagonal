using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Domain.Events
{
    public class RentVehicleReturnedEvent : IDomainEvent
    {
        public RentVehicleReturnedEvent(Guid rentVehicleId)
        {
            EventId = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
            RentVehicleId = rentVehicleId;
        }

        public Guid EventId { get; set; }

        public DateTime OccurredOnUtc { get; set; }

        public Guid RentVehicleId { get; set; }
    }
}

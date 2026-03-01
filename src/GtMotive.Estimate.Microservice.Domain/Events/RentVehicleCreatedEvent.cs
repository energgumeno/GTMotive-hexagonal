using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Domain.Events
{
    public class RentVehicleCreatedEvent : IDomainEvent
    {
        public RentVehicleCreatedEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        public Guid EventId { get; set; }

        public DateTime OccurredOnUtc { get; set; }
    }
}

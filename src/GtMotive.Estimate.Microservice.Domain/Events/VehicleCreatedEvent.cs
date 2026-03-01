using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Events
{
    public class VehicleCreatedEvent( Vehicle vehicle) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();

        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

        private Vehicle vehicle { get; } = vehicle;
    }
}

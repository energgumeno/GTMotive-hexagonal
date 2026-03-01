using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Events
{
    public class RentVehicleCreatedEvent(RentInformation rentInformation) : IDomainEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; set; } = DateTime.UtcNow;
        public RentInformation RentInformation { get; set; } = rentInformation;
    }
}
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Events;

public class RentVehicleReturnedEvent(RentInformation rentInformation) : IDomainEvent
{
    public RentInformation RentInformation { get; set; } = rentInformation;
    public Guid EventId { get; set; } = Guid.NewGuid();

    public DateTime OccurredOnUtc { get; set; } = DateTime.UtcNow;
}
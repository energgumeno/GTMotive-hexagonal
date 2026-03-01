using System;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Domain.Events
{
    public class VehicleCreatedEvent : IDomainEvent
    {
        public Guid EventId { get; }

        public DateTime OccurredOnUtc { get; }

        /// <summary>
        /// Gets Registration date.
        /// </summary>
        public DateTime RegistrationDate { get; }

        /// <summary>
        /// Gets Frame id.
        /// </summary>
        public string FrameId { get; }

        /// <summary>
        /// Gets License Plate.
        /// </summary>
        public string LicensePlate { get; }
    }
}

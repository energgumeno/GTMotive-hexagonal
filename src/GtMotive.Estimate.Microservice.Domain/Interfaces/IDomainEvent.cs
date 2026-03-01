using System;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// A domain event.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the event.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// Gets the date and time when the event occurred (UTC).
        /// </summary>
        DateTime OccurredOnUtc { get; }
    }
}

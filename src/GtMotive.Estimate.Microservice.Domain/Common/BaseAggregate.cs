using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// A base entity.
    /// </summary>
    public abstract class BaseAggregate
    {
        /// <summary>
        /// Gets or sets Unique Id.
        /// </summary>
        public Guid Id { get; set; }
    }
}

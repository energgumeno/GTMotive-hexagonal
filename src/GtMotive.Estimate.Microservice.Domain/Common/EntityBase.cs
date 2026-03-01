using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Domain.Common;

/// <summary>
///     Entity base for events.
/// </summary>
public class EntityBase
{
    /// <summary>
    ///     The list of domain events.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    ///     Gets List of domain events.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Gets a value indicating whether count of Domain events is 0.
    /// </summary>
    public bool HasDomainEvents => _domainEvents.Count != 0;


    /// <summary>
    ///     Clears all domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    ///     Adds a domain event to be processed.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }


    /// <summary>
    ///     Removes a specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
}
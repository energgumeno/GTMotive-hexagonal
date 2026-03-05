using System.Linq.Expressions;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port;

/// <summary>
///     Defines write operations for managing vehicles in the system.
/// </summary>
/// <remarks>
///     This service is responsible for creating and persisting vehicle records in the underlying data source.
///     It extends read service capabilities and provides transactional support through the unit of work pattern.
/// </remarks>
public interface IVehiclePort : IUnitOfWork
{
    Task<Vehicle?> GetVehicle(Expression<Func<Vehicle, bool>> filter);
    Task<(List<Vehicle>, int)> GetVehicles(Expression<Func<Vehicle, bool>> filter, int pageIndex,
        int pageSize);
    Task<List<Vehicle>> GetVehicles(Expression<Func<Vehicle, bool>> filter);
    Task<Guid> AddVehicle(Vehicle vehicle);
}
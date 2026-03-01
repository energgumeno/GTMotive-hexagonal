using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port
{
    /// <summary>
    /// Defines write operations for managing vehicles in the system.
    /// </summary>
    /// <remarks>
    /// This service is responsible for creating and persisting vehicle records in the underlying data source.
    /// It extends read service capabilities and provides transactional support through the unit of work pattern.
    /// </remarks>
    public interface IVehiclePort:IUnitOfWork
    {
        Task<List<Vehicle?>> GetVehicles(int pageIndex, int pageSize);
        Task<Vehicle?> GetVehicle(Guid vehicleId);
        Task<Vehicle?> GetVehicle(Vehicle vehicle);
        Task<Guid> AddVehicle(Vehicle  vehicle);
    }
}

using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Services
{
    public interface IRentVehicleService:IUnitOfWork
    {
        Task<List<RentInformation>> GetVehiclesRent(int pageIndex, int pageSize);
        Task<List<RentInformation>> GetVehicleRent(Guid vehicleId);
        Task<Guid> AddVehicleRent(RentInformation rentInformation);
    }
}
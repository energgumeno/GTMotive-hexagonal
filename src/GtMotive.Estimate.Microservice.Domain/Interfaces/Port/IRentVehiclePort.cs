using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port;

public interface IRentVehiclePort : IUnitOfWork
{
    Task<(List<RentInformation?>, int)> GetVehiclesRent(int pageIndex, int pageSize);
    Task<RentInformation?> GetVehicleRentByRentId(Guid vehicleId);
    Task<List<RentInformation>> GetVehiclesRentByVehicleId(Guid vehicleId);
    Task<List<RentInformation>> GetVehicleRentByEmail(string email);
    Task<Guid?> AddVehicleRent(RentInformation rentInformation);
    Task<Guid?> UpdateVehicleRent(RentInformation rentInformation);
}
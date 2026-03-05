using System.Linq.Expressions;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port;

public interface IRentVehiclePort : IUnitOfWork
{
    Task<(List<RentInformation>, int)> GetVehiclesRent(int pageIndex, int pageSize);

    Task<RentInformation?> GetVehicleRent(Expression<Func<RentInformation, bool>> filter);

    Task<(List<RentInformation>, int)> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter, int pageIndex,
        int pageSize);

    Task<List<RentInformation>> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter);
    
    // Task<RentInformation?> GetVehicleRentByRentId(Guid vehicleId);
    // Task<List<RentInformation>> GetVehiclesRentByVehicleId(Guid vehicleId);
    // Task<List<RentInformation>> GetVehicleRentByEmail(string email);
    Task<Guid?> AddVehicleRent(RentInformation rentInformation);
    Task<Guid?> UpdateVehicleRent(RentInformation rentInformation);
}
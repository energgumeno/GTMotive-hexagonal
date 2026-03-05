using System.Linq.Expressions;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port;

public interface IRentVehiclePort : IUnitOfWork
{
    Task<RentInformation?> GetVehicleRent(Expression<Func<RentInformation, bool>> filter);

    Task<(List<RentInformation>, long)> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter, int pageIndex,
        int pageSize);

    Task<List<RentInformation>> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter);

    Task AddVehicleRent(RentInformation rentInformation);
    Task UpdateVehicleRent(RentInformation rentInformation);
}